using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using VRTK;

public class GrabOnPoint : MonoBehaviour {

    public VRTK_Pointer pointer;

    VRTK_ControllerEvents controllerEvents;

    void OnPointerValid( object sender, DestinationMarkerEventArgs e )
    {
        if( controllerEvents == null 
            || controllerEvents.GetGripAxis() < 0.6f ) { return; }

        VRTK_InteractableObject obj = e.raycastHit.collider.gameObject.GetComponent<VRTK_InteractableObject>();
        if( obj != null && obj.isGrabbable && !obj.IsGrabbed() ) {
        }
    }

    private void Start()
    {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();

        if( pointer != null ) {
            pointer.PointerStateValid += OnPointerValid;
        }
    }
}
