using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class GameMenu : MonoBehaviour
{
    public string sceneName = "Menu";
    public string VRTKSceneName = "VRTK";

    List<GameObject> stashedRoots = new List<GameObject>();
    Vector3 storedPosition;
    Quaternion storedRotation;
    VRTK_ControllerEvents.ButtonAlias menuButton = VRTK_ControllerEvents.ButtonAlias.StartMenuPress;
    VRTK_ControllerEvents controllerEvents;

    void SubscribeMenuButton()
    {
        if( controllerEvents == null ) { return; }
        
        controllerEvents.SubscribeToButtonAliasEvent( menuButton, true, ToggleMenu );
    }

    void ToggleMenu( object sender, ControllerInteractionEventArgs e )
    {
        // Find current scene state
        Scene scene = SceneManager.GetSceneByName( sceneName );
        if( !scene.IsValid() ) {
            LoadMenu();
        } else {
            UnloadMenu();
        }
    }

    void LoadMenu()
    {
        Transform playArea = VRTK_DeviceFinder.PlayAreaTransform();
        storedPosition = playArea.position;
        storedRotation = playArea.rotation;

        for( int i = 0; i < SceneManager.sceneCount; i++ ) {
            Scene scene = SceneManager.GetSceneAt( i );
            if( scene.name != VRTKSceneName ) {
                GameObject[] roots = scene.GetRootGameObjects();
                foreach( GameObject obj in roots ) {
                    obj.SetActive( false );
                    stashedRoots.Add( obj );
                }
            }
        }

        AsyncOperation async = SceneManager.LoadSceneAsync( sceneName, LoadSceneMode.Additive );
    }
    void UnloadMenu()
    {
        if( stashedRoots.Count == 0 || storedPosition == null || storedRotation == null ) { return; }

        SceneManager.UnloadSceneAsync( sceneName );

        Transform playArea = VRTK_DeviceFinder.PlayAreaTransform();
        playArea.position = storedPosition;
        playArea.rotation = storedRotation;

        foreach( GameObject obj in stashedRoots ) {
            obj.SetActive( true );
        }
        stashedRoots.Clear();
    }

    private void Start()
    {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
        SubscribeMenuButton();
    }
}
