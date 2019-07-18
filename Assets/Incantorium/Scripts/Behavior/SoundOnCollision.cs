using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnCollision : MonoBehaviour
{
    public AudioClip collisionSound;
    public float minimumVelocity = 0.5f;
    public bool velocityAffectsVolume = true;
    public float pitchVariation = 0.4f;

    AudioSource audioSource;
    Rigidbody rigidbody;

	// Use this for initialization
	void OnEnable ()
    {
		audioSource = GetComponent<AudioSource>();
        if( audioSource == null ) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        rigidbody = GetComponent<Rigidbody>();
        if( rigidbody == null ) {
            rigidbody = gameObject.AddComponent<Rigidbody>();
        }

        audioSource.clip = collisionSound;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if( !this.enabled ) { return; }

        if( rigidbody.velocity.magnitude >= minimumVelocity ) {
            audioSource.volume = rigidbody.velocity.magnitude / 1;
            audioSource.pitch = Random.Range(1 - pitchVariation, 1 + pitchVariation);

            audioSource.PlayOneShot(audioSource.clip);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if( !this.enabled ) { return; }

        if (rigidbody.velocity.magnitude >= minimumVelocity)
        {
            audioSource.volume = rigidbody.velocity.magnitude / 1;
            audioSource.pitch = Random.Range(1 - pitchVariation, 1 + pitchVariation);

            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
