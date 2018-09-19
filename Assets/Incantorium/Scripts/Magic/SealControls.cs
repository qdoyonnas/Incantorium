using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SealControls : MonoBehaviour
{
    public VRTK_Pointer interactionPointer;
    public float focusRange = 0.1f;

    bool sealFocused = false;
    bool sealGrabbed = false;

    public bool GetSealFocused()
    {
        return sealFocused;
    }
    public bool GetSealGrabbed()
    {
        return sealGrabbed;
    }

    Magic magicScript;
    VRTK_ControllerEvents controllerEvents;
    Seal heldSealScript;

    Vector3 savedPosition = Vector3.zero;
    Vector3 savedRotation = Vector3.zero;

    public void ResetSavedVectors()
    {
        savedPosition = transform.position;
        savedRotation = transform.localEulerAngles;
    }
    public Vector3 GetPositionDelta()
    {
        if( !sealFocused ) { return Vector3.zero; }

        Vector3 delta = transform.position - savedPosition;
        savedPosition = transform.position;

        return delta;
    }
    public Vector3 GetRotationDelta()
    {
        if( !sealFocused ) { return Vector3.zero; }

        Vector3 delta = transform.localEulerAngles - savedRotation;
        savedRotation = transform.localEulerAngles;

        return delta;
    }

    void FocusSeal( object sender, ControllerInteractionEventArgs e )
    {
        sealFocused = false;

        if( controllerEvents.GetTriggerAxis() >= 0.5f - focusRange
            && ( magicScript.FocusActive || controllerEvents.GetTriggerAxis() <= 0.5f + focusRange) )
        {
            sealFocused = true;
        }
    }
    void GrabSeal( object sender, ControllerInteractionEventArgs e )
    {
        sealGrabbed = false;

        if( controllerEvents.GetGripAxis() >= 0.5f - focusRange
            && ( magicScript.FocusActive || controllerEvents.GetGripAxis() <= 0.5f + focusRange) )
        {
            sealGrabbed = true;
        }
    }

    bool CheckInteractGrabStatus()
    {
        VRTK_InteractGrab grabber = GetComponent<VRTK_InteractGrab>();
        if( grabber != null && grabber.GetGrabbedObject() != null ) { return true; }

        return false;
    }

    void SubscribeToControllerEvents()
    {
        if( controllerEvents == null ) { return; }

        controllerEvents.TriggerAxisChanged += FocusSeal;
        controllerEvents.GripAxisChanged += GrabSeal;
    }
    void Start()
    {
        magicScript = VRTKScripts.instance.GetComponentInChildren<Magic>();

        controllerEvents = GetComponent<VRTK_ControllerEvents>();
        SubscribeToControllerEvents();
    }
}
