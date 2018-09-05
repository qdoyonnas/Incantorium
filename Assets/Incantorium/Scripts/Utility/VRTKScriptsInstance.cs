using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VRTKScripts
{
    public static GameObject instance;
}

public class VRTKScriptsInstance : MonoBehaviour
{
    private void Awake()
    {
        if( VRTKScripts.instance != null ) { return; }

        VRTKScripts.instance = this.gameObject;
    }
}