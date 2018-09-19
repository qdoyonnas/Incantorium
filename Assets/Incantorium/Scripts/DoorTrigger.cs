using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorTrigger : MonoBehaviour
{
    public bool active = true;
    public GameObject winText;

    AudioSource audioSource;

    public void Trigger()
    {
        if( !active ) { return; }
        active = false;

        audioSource.Play();
        transform.DOMove( transform.position - new Vector3(0, 3, 0), 4.5f )
            .OnComplete( () => winText.SetActive( true ) );
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
