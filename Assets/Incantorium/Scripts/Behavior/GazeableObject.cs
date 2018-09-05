using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public abstract class GazeableObject : MonoBehaviour
{
    public float gazeDistance = 0.3f;

    protected bool isGazedAt = false;
    protected float gazeDuration = 0;
    protected Transform gazingObject;

    bool InGazeRange( Transform obj )
    {
        if( obj == null ) { return false; }
        return ( Vector3.Distance(transform.position, obj.position) <= gazeDistance );
    }

    public virtual void DoGazeStart( GazeRegister gazeRegister, Transform headset )
    {
        isGazedAt = true;
        gazingObject = headset;
    }
    public virtual void DoGazeStop( GazeRegister gazeRegister, Transform headset )
    {
        isGazedAt = false;
        gazingObject = null;
    }
    protected abstract void DoGazeAction();

    protected virtual void UpdateGaze()
    {
        if( isGazedAt && InGazeRange( gazingObject ) ) {
            gazeDuration += Time.deltaTime;
            DoGazeAction();
        } else {
            gazeDuration = 0;
        }
    }

    private void Update()
    {
        UpdateGaze();
    }
}
