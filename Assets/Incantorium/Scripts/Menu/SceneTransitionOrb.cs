using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class SceneTransitionOrb : MenuOrb
{
    public string sceneName;
    public LoadSceneMode loadType = LoadSceneMode.Single;

    Scene VRTK_scene;

    void UnloadOtherScenes()
    {
        if( loadType == LoadSceneMode.Additive ) { return; }

        VRTK_scene = VRTKScripts.instance.scene;

        for( int i = 0; i < SceneManager.sceneCount; i++ ) {
            Scene scene = SceneManager.GetSceneAt( i );
            if( scene != VRTK_scene ) { SceneManager.UnloadSceneAsync( scene ); }
        }
    }

    protected override void DoFinalAction()
    {
        if( string.IsNullOrEmpty(sceneName) ) { return; }

        UnloadOtherScenes();
        SceneManager.LoadScene( sceneName, LoadSceneMode.Additive );
    }
}
