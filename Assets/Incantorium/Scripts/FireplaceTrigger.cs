using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplaceTrigger : MonoBehaviour
{
    public GameObject crystal;
    public GameObject boxLid;
    public AudioSource audioSource;

    FlameObject flameObject;

    void DoOnLight( bool isLit )
    {
        boxLid.SetActive( false );
        crystal.SetActive( true );

        audioSource.Play();
    }

    private void Start()
    {
        flameObject = GetComponent<FlameObject>();
        flameObject.OnLight += DoOnLight;
    }
}
