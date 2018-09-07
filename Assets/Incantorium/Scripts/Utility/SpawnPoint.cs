using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SpawnPoint : MonoBehaviour
{
    private void Start()
    {
        VRTK_HeadsetFade fade = VRTKScripts.instance.GetComponentInChildren<VRTK_HeadsetFade>();
        if( fade != null ) {
            fade.Fade( Color.black, 0 );
        }

        Transform playArea = VRTK_DeviceFinder.PlayAreaTransform();

        playArea.position = this.transform.position;
        playArea.rotation = this.transform.rotation;

        OnStart();

        if( fade != null ) {
            fade.Unfade( 0.5f );
        }
    }

    protected virtual void OnStart() {}
}
