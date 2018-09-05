using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public string String;
    public bool isTag;
    public bool XAxis = true;
    public bool YAxis = true;
    public bool lookAway = false;

    GameObject target;

    private void Start()
    {
        FindTarget();
    }

    void FindTarget()
    {
        if( isTag ) {
            GameObject[] cameras = GameObject.FindGameObjectsWithTag( String );
            foreach( GameObject obj in cameras ) {
                if( obj.activeSelf ) {
                    target = obj;
                    return;
                }
            }
        } else {
            target = GameObject.Find( String );
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if( target == null || !target.activeSelf ) {
            FindTarget();
        }
        if( target == null || !target.activeSelf ) { return; }

        Vector3 angles = transform.eulerAngles;
        transform.LookAt(target.transform.position);

        if( lookAway ) {
            transform.Rotate( Vector3.up, 180, Space.Self );
        }

        if( !XAxis || !YAxis ) {
            Vector3 newAngles = transform.eulerAngles;
            transform.eulerAngles = new Vector3( ( !XAxis ? angles.x : newAngles.x ), ( !YAxis ? angles.y : newAngles.y ), newAngles.z );
        }
	}
}
