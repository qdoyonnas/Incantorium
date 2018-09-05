using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class GameMenu : MonoBehaviour
{
    public GameObject menuObject;

    VRTK_ControllerEvents.ButtonAlias menuButton = VRTK_ControllerEvents.ButtonAlias.StartMenuPress;
    VRTK_ControllerEvents controllerEvents;

    void SubscribeMenuButton()
    {
        if( controllerEvents == null ) { return; }

        controllerEvents.SubscribeToButtonAliasEvent( menuButton, true, ToggleMenu );
    }

    void ToggleMenu( object sender, ControllerInteractionEventArgs e )
    {
        if( menuObject.activeSelf ) {
            menuObject.SetActive( false );
        } else {
            menuObject.SetActive( true );
        }
    }

    private void Start()
    {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
        SubscribeMenuButton();
    }
}
