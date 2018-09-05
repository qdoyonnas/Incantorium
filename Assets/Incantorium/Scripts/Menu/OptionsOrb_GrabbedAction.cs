using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class OptionsOrb_GrabbedAction : MonoBehaviour
{
    public GameObject contextObject;

    VRTK_InteractableObject interactable;

    void OnGrabbed( object sender, InteractableObjectEventArgs e )
    {
        contextObject.SetActive( true );
    }
    void OnGrabRelease( object sender, InteractableObjectEventArgs e )
    {
        contextObject.SetActive( false );
    }

    void SubscribeToInteractable()
    {
        if( interactable == false ) { return; }

        interactable.SubscribeToInteractionEvent( VRTK_InteractableObject.InteractionType.Grab, OnGrabbed );
        interactable.SubscribeToInteractionEvent( VRTK_InteractableObject.InteractionType.Ungrab, OnGrabRelease );
    }

    private void Start()
    {
        interactable = GetComponent<VRTK_InteractableObject>();
        SubscribeToInteractable();

        if( contextObject != null ) {
            contextObject.SetActive( false );
        }
    }
}
