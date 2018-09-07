using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSealsOnSpawn : SpawnPoint
{
    public bool doEnable = true;

    protected override void OnStart()
    {
        Component[] components = VRTKScripts.instance.GetComponentsInChildren<SealControls>();
        foreach( Component component in components ) {
            SealControls sealControl = (SealControls)component;
            sealControl.Enabled = doEnable;
        }
    }
}
