using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class GazeRegister : MonoBehaviour
{
    public VRTK_HeadsetControllerAware headsetAwareness;

    VRTK_InteractGrab interactGrab;
    bool isGazing = false;

    #region ControllerAwareness Methods

    void OnControllerAware( object sender, HeadsetControllerAwareEventArgs e )
    {
        isGazing = true;
        DoControllerAware( sender, e, true );
    }
    void OnControllerUnaware( object sender, HeadsetControllerAwareEventArgs e )
    {
        isGazing = false;
        DoControllerAware( sender, e, false );
    }
    void DoControllerAware( object sender, HeadsetControllerAwareEventArgs e, bool isStart )
    {
        if( e.controllerReference.scriptAlias == gameObject ) {
            GazeableOjectInteract( isStart );
        }
    }

    void GazeableOjectInteract(bool isStart)
    {
        GazeableObject grabbedGazeableObject = GetGrabbedGazeableObject();
        if( grabbedGazeableObject != null ) {
            if( isStart ) {
                grabbedGazeableObject.DoGazeStart( this, VRTK_DeviceFinder.HeadsetTransform() );
            } else {
                grabbedGazeableObject.DoGazeStop( this, VRTK_DeviceFinder.HeadsetTransform() );
            }
        }
    }

    #endregion

    #region Grabbed Methods

    GameObject GetGrabbedObject()
    {
        if( interactGrab == null ) { return null; }

        return interactGrab.GetGrabbedObject();
    }
    GazeableObject GetGrabbedGazeableObject()
    {
        GameObject grabbedObject = GetGrabbedObject();
        return ( grabbedObject == null ? null : grabbedObject.GetComponent<GazeableObject>() );
    }

    void OnControllerGrab( object sender, ObjectInteractEventArgs e )
    {
        if( isGazing ) {
            GazeableOjectInteract( true );
        }
    }
    void OnControllerGrabRelease( object sender, ObjectInteractEventArgs e )
    {
        if( isGazing ) {
            GazeableOjectInteract( false );
        }
    }

    #endregion

    #region Subscription Methods

    void FindHeadsetAwareness()
    {
        if( headsetAwareness != null ) { return; }

        if( VRTKScripts.instance == null ) { return; }

        VRTKScripts.instance.GetComponentInChildren<VRTK_HeadsetControllerAware>();
    }

    void SubscribeToHeadsetAwareness()
    {
        if( headsetAwareness == null ) { return; }

        headsetAwareness.ControllerGlanceEnter += OnControllerAware;
        headsetAwareness.ControllerGlanceExit += OnControllerUnaware;
    }

    void SubscribeToInteractGrab()
    {
        if( interactGrab == null ) { return; }

        interactGrab.ControllerStartGrabInteractableObject += OnControllerGrab;
        interactGrab.ControllerStartUngrabInteractableObject += OnControllerGrabRelease;
    }

    #endregion

    private void Start()
    {
        interactGrab = GetComponent<VRTK_InteractGrab>();

        FindHeadsetAwareness();
        SubscribeToHeadsetAwareness();
        SubscribeToInteractGrab();
    }
}
