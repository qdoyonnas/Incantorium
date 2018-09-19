using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using VRTK;

public class Magic : MonoBehaviour
{
    public delegate void OnFocusDelegate();
    public delegate void OnUnfocusDelegate();

    public event OnFocusDelegate OnFocus;
    public event OnUnfocusDelegate OnUnfocus;

    public PostProcessVolume magicProcessing;
    public float focusDuration = 0.2f;
    public float depthOfFieldRange = 50;
    public float vignetteRange = 0.5f;

    Camera magicCamera;
    SealControls leftSealControls;
    SealControls rightSealControls;
    float focusStart = 0;
    bool focusActive = false;

    Seal selectedSeal = null;

    public bool FocusActive
    {
        get {
            return focusActive;
        }
        set {
            if( value && !focusActive ) {
                Focus();
            } else if( !value && focusActive ) {
                Unfocus();
            }

            focusActive = value;
        }
    }

    bool FindMagicCamera()
    {
        if( magicCamera != null ) { return true; }

        GameObject[] objects = GameObject.FindGameObjectsWithTag( "MagicCamera" );
        foreach( GameObject obj in objects ) {
            if( obj.activeSelf ) {
                magicCamera = obj.GetComponent<Camera>();
                if( magicCamera != null ) { return true; }
            }
        }

        return false;
    }

    public Camera GetMagicCamera()
    {
        FindMagicCamera();

        return magicCamera;
    }

    bool GetSealFocused()
    {
        bool state = false;

        if( leftSealControls != null ) {
            state = state || leftSealControls.GetSealFocused();
        }
        if( rightSealControls != null ) {
            state = state || rightSealControls.GetSealFocused();
        }

        return state;
    }
    bool GetSealGrabbed()
    {
        bool state = false;

        if( leftSealControls != null ) {
            state = leftSealControls.GetSealGrabbed();
        }
        if( rightSealControls != null ) {
            state = state || rightSealControls.GetSealGrabbed();
        }

        return state;
    }

    public void Focus()
    {
        Transform playTransform = VRTK_DeviceFinder.PlayAreaTransform();

        GameObject[] magicObjects = GameObject.FindGameObjectsWithTag( "MagicObject" );
        MagicObject selectedObject = null;
        float smallestDistance = float.PositiveInfinity;
        foreach( GameObject obj in magicObjects ) {
            MagicObject script = obj.GetComponent<MagicObject>();
            if( script == null || !script.isOwned ) { continue; }

            float distance = Vector3.Distance( playTransform.position, script.transform.position );
            if( distance > 4f || distance > smallestDistance ) { continue; }

            selectedObject = script;
            smallestDistance = distance;
        }

        if( selectedObject != null ) {
            selectedSeal = selectedObject.DisplaySeal( true );
            leftSealControls.ResetSavedVectors();
            rightSealControls.ResetSavedVectors();
        }

        if( OnFocus != null ) {
            OnFocus();
        }
    }
    public void Unfocus()
    {
        selectedSeal = null;

        if( OnUnfocus != null ) {
            OnUnfocus();
        }
    }

    void SetVignette( float intensity )
    {
        if( magicProcessing == null ) { return; }

        Vignette vignette = ScriptableObject.CreateInstance<Vignette>();

        vignette.enabled.Override( (intensity > 0 ? true : false ) );
        vignette.intensity.Override( intensity * vignetteRange );

        magicProcessing.profile.AddSettings( vignette );
    }
    void SetDepthOfField( float intensity )
    {
        if( magicProcessing == null ) { return; }

        DepthOfField depthOfField = ScriptableObject.CreateInstance<DepthOfField>();

        depthOfField.enabled.Override( (intensity > 0 ? true : false) );
        depthOfField.focalLength.Override( intensity * depthOfFieldRange );

        magicProcessing.profile.AddSettings( depthOfField );
    }

    bool CheckInputs()
    {
        bool sealFocused = GetSealFocused();
        bool sealGrabbed = GetSealGrabbed();

        return sealFocused && sealGrabbed;
    }
    void StartFocusCharge()
    {
        focusStart = Time.time;

        SetDepthOfField( 0.01f );
    }
    void FocusCharge()
    {
        if( Time.time > focusStart + focusDuration ) {
            FocusActive = true;
        } else {
            float intensity = ( Time.time - focusStart ) / focusDuration;
            SetVignette( intensity );
        }
    }
    void EndFocusCharge()
    {
        focusStart = 0;
        FocusActive = false;

        SetVignette( 0 );
    }
    void Charge()
    {
        if( GetSealFocused() ) {
            if( focusStart == 0 ) {
                StartFocusCharge();
            } else {
                FocusCharge();
            }
        } else if( focusStart != 0 ) {
            EndFocusCharge();
        }
    }

    void RunInputs()
    {
        if( !FocusActive || selectedSeal == null ) { return; }

        Vector3 positionDelta = Vector3.zero;
        Vector3 rotationDelta = Vector3.zero;

        Vector3 leftPosDelta = leftSealControls.GetPositionDelta();
        Vector3 rightPosDelta = rightSealControls.GetPositionDelta();
        positionDelta = (leftPosDelta.magnitude > rightPosDelta.magnitude ? leftPosDelta : rightPosDelta);

        Vector3 leftRotDelta = leftSealControls.GetRotationDelta();
        Vector3 rightRotDelta = rightSealControls.GetRotationDelta();
        rotationDelta = (leftRotDelta.magnitude > rightRotDelta.magnitude ? leftRotDelta : rightRotDelta);

        selectedSeal.RunInputs( positionDelta, rotationDelta );
    }

    private void Update()
    {
        Charge();
    }

    private void FixedUpdate()
    {
        RunInputs();
    }

    private void Start()
    {
        leftSealControls = VRTKScripts.instance.transform.Find( "LeftController" ).GetComponent<SealControls>();
        rightSealControls = VRTKScripts.instance.transform.Find( "RightController" ).GetComponent<SealControls>();

        FindMagicCamera();

        EndFocusCharge();
    }
}
