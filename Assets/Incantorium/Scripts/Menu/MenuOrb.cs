using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using DG.Tweening;

public class MenuOrb : GazeableObject
{
    #region Public Fields

    public float tweenSpeed = 0.5f;
    public float anchorRange = 0.1f;
    public Transform anchor;

    public Color fadeColor = Color.black;
    public float gazeTriggerDuration = 1f;
    public float unfadeRate = 0.1f;

    #endregion

    #region Private Fields

    bool isAnchored;
    HoverInPlace hover;
    VRTK_InteractableObject interactable;
    GameObject glowEffect;
    Light glowLight;

    bool isDoingAction = false;
    VRTK_HeadsetFade headsetFade;

    Tweener transformTween;

    #endregion

    #region Helper Mehods

    public bool IsGrabbed
    {
        get {
            return interactable != null && interactable.IsGrabbed();
        }
    }
    bool CheckDuration()
    {
        return gazeDuration >= gazeTriggerDuration;
    }
    void Fade(bool fadeOut)
    {
        if( headsetFade == null || fadeColor.a == 0 ) { return; }

        if( fadeOut ) {
            headsetFade.Fade( Color.black, gazeTriggerDuration );
        } else {
            headsetFade.Unfade( unfadeRate );
        }
    }

    #endregion

    #region GazeAction Methods

    protected virtual void DoFinalAction()
    {
        // Do nothing
    }

    protected override void DoGazeAction()
    {
        if( isDoingAction ) { return; }

        isDoingAction = true;
        Fade( true );
    }
    protected virtual void UpdateGazeActions()
    {
        if( isDoingAction ) {
            if( gazeDuration == 0 ) {
                isDoingAction = false;
                Fade( false );
            } else if( CheckDuration() ) {
                DoFinalAction();
            }
        }
    }

    #endregion

    #region Position Methods

    void SetHover( bool state )
    {
        if( hover == null ) { return; }

        hover.isEnabled = state;
    }
    void StartTween()
    {
        if( anchor == null || transformTween != null ) { return; }

        transformTween = transform.DOMove( anchor.position, tweenSpeed )
            .SetEase( Ease.InOutCubic )
            .OnKill( () => transformTween = null );
    }
    bool CheckAnchor()
    {
        return ( Vector3.Distance( transform.position, anchor.position ) < anchorRange ) ;
    }

    void UpdatePosition()
    {
        isAnchored = CheckAnchor();
        SetHover( !IsGrabbed && isAnchored );

        if( !isAnchored && !IsGrabbed ) {
            StartTween();
        }
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        hover = GetComponent<HoverInPlace>();
        interactable = GetComponent<VRTK_InteractableObject>();
        glowEffect = transform.Find( "GlowEffect" ).gameObject;
        glowLight = GetComponentInChildren<Light>();

        headsetFade = ( VRTKScripts.instance == null ? null : VRTKScripts.instance.GetComponentInChildren<VRTK_HeadsetFade>() );
        isAnchored = transform.position == anchor.position;
        SetHover( isAnchored );
    }

    private void Update()
    {
        UpdatePosition();

        UpdateGaze();

        UpdateGazeActions();
    }

    #endregion
}
