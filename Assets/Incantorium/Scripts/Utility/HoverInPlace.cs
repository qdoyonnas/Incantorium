using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverInPlace : MonoBehaviour
{
    public bool isEnabled = true;
    public bool randomOffset = true;
    public float offset = 0;
    public float factor = 0.2f;

    Vector3 startPosition;

	// Use this for initialization
	void Start ()
    {
        startPosition = transform.position;

        if( randomOffset ) {
            offset = Random.value;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if( !isEnabled ) { return; }

        transform.position = new Vector3( startPosition.x, startPosition.y + ( Mathf.Sin( Time.time + offset ) * factor ), startPosition.z );
	}
}
