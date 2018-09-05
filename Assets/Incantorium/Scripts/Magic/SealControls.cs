using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SealControls : MonoBehaviour
{
    #region Public Fields

    public Seal sealScript;
    public VRTK_ControllerEvents.ButtonAlias focusButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
    public VRTK_ControllerEvents.ButtonAlias grabButton = VRTK_ControllerEvents.ButtonAlias.GripPress;

    #endregion

    #region Private Fields

    [SerializeField]
    bool _enabled = true;

    bool sealFocused = false;
    bool sealGrabbed = false;

    VRTK_ControllerEvents.ButtonAlias subscribedFocusButton;
    VRTK_ControllerEvents.ButtonAlias subscribedGrabButton;
    VRTK_ControllerEvents controllerEvents;

    #endregion

    #region Properties

    public bool Enabled
    {
        get {
            return _enabled;
        }
        set {
            _enabled = value;
            if( _enabled ) {
                SubscribeToEvents();
                if( sealScript != null ) { sealScript.SetActive( true ); }
            } else {
                UnsubscribeFromEvents();
                if( sealScript != null ) { sealScript.SetActive( false ); }
            }
        }
    }

    #endregion

    #region Subscription Methods

    void SubscribeToEvents()
    {
        SubscribeToFocusButton();
        SubscribeToGrabButton();
    }
    void SubscribeToFocusButton()
    {
        if( controllerEvents == null
            || focusButton == VRTK_ControllerEvents.ButtonAlias.Undefined
            || focusButton == subscribedFocusButton )
        { return; }

        controllerEvents.SubscribeToButtonAliasEvent( focusButton, true, FocusSeal );
        controllerEvents.SubscribeToButtonAliasEvent( focusButton, false, ReleaseFocus );
        subscribedFocusButton = focusButton;
    }
    void SubscribeToGrabButton()
    {
        if( controllerEvents == null
            || grabButton == VRTK_ControllerEvents.ButtonAlias.Undefined
            || grabButton == subscribedGrabButton ) 
        { return; }

        controllerEvents.SubscribeToButtonAliasEvent( grabButton, true, GrabSeal );
        controllerEvents.SubscribeToButtonAliasEvent( grabButton, false, ReleaseGrab );
        subscribedGrabButton = grabButton;
    }

    #endregion

    #region Unsubscription Methods

    void UnsubscribeFromEvents()
    {
        UnsubscribeFromFocusButton();
        UnsubscribeFromGrabButton();
    }
    void UnsubscribeFromFocusButton()
    {
        if( controllerEvents == null
            || subscribedFocusButton == VRTK_ControllerEvents.ButtonAlias.Undefined)
        { return; }

        controllerEvents.UnsubscribeToButtonAliasEvent( focusButton, true, FocusSeal );
        controllerEvents.UnsubscribeToButtonAliasEvent( focusButton, false, ReleaseFocus );
        subscribedFocusButton = focusButton;
    }
    void UnsubscribeFromGrabButton()
    {
        if( controllerEvents == null
            || subscribedGrabButton == VRTK_ControllerEvents.ButtonAlias.Undefined)
        { return; }

        controllerEvents.UnsubscribeToButtonAliasEvent( grabButton, true, GrabSeal );
        controllerEvents.UnsubscribeToButtonAliasEvent( grabButton, false, ReleaseGrab );
        subscribedGrabButton = grabButton;
    }

    #endregion

    #region Focus Methods

    void FocusSeal( object sender, ControllerInteractionEventArgs e )
    {
        // Confirm the hand isn't grabbing any interactable object
        // And we aren't already focusing a seal
        if( CheckInteractGrabStatus()
            || sealFocused )
        { return; }

        // Determine if we are focusing a seal or creating one
        if( sealScript.isActive ) {
            controllerEvents = GetComponent<VRTK_ControllerEvents>();
            SubscribeToEvents();
        } else {
            sealScript.Create( this );
        }

        sealFocused = true;
    }
    void ReleaseFocus( object sender, ControllerInteractionEventArgs e )
    {
        if( !sealFocused ) { return; }
        
        sealFocused = false;
    }

    #endregion

    #region Grab Methods

    void GrabSeal( object sender, ControllerInteractionEventArgs e )
    {
        // Confirm the hand isn't grabbing any interactable object
        // And we aren't already holding a seal
        if( CheckInteractGrabStatus()
            || sealGrabbed )
        { return; }

        //
    }
    void ReleaseGrab( object sender, ControllerInteractionEventArgs e )
    {

    }

    #endregion

    #region Helper Methods

    bool CheckInteractGrabStatus()
    {
        VRTK_InteractGrab grabber = GetComponent<VRTK_InteractGrab>();
        if( grabber != null && grabber.GetGrabbedObject() != null ) { return true; }

        return false;
    }

    #endregion

    #region Unity Methods

    void Start()
    {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();

        if( _enabled ) {
            SubscribeToEvents();
            if( sealScript != null ) { sealScript.SetActive( true ); }
        } else {
            if( sealScript != null ) { sealScript.SetActive( false ); }
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if( !_enabled ) { return; }

        if( sealScript != null && sealScript.isActive && sealFocused ) {
            sealScript.MoveTo( this.transform.position );
        }
	}

    #endregion
}
