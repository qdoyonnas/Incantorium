using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class FlameObject : MonoBehaviour
{
    public bool isLightable = true;
    public bool canTransferFlame = true;
    public bool isExtinguishable = true;

    public delegate void LightDelegate( bool isLit );
    public event LightDelegate OnLight;

    [SerializeField]
    bool isLit = false;

    public bool IsLit
    {
        get {
            return isLit;
        }
        set {
            isLit = value;
            UpdateEffects( value );
            SetMagic( value );

            if( OnLight != null ) {
                OnLight( value );
            }
        }
    }

    Light[] flameLights;
    ParticleSystem[] particles;
    SphereCollider[] heatTriggers;
    AudioSource[] sounds;

    private void Start()
    {
        flameLights = GetComponentsInChildren<Light>();
        particles = GetComponentsInChildren<ParticleSystem>();
        heatTriggers = GetComponentsInChildren<SphereCollider>();
        sounds = GetComponentsInChildren<AudioSource>();

        UpdateEffects( isLit );
    }

    void SetMagic( bool state )
    {
        MagicObject magic = GetComponent<MagicObject>();
        if( magic == null ) { return; }
        magic.isOwned = true;

        if( state ) {
            magic.AddSigil( Sigil.Fire );
        } else {
            magic.RemoveSigil( Sigil.Fire );
        }
    }

    void UpdateEffects( bool state )
    {
        SetFlameLight( state );
        SetParticles( state );
        SetSound( state );
    }
    void SetSound( bool state )
    {
        if( sounds == null || sounds.Length == 0 ) { return; }

        foreach( AudioSource sound in sounds ) {
            if( state ) {
                sound.Play();
            } else {
                sound.Stop();
            }
        }
    }
    void SetFlameLight( bool state )
    {
        if( flameLights == null || flameLights.Length == 0 ) { return; }

        foreach( Light flameLight in flameLights ) {
            flameLight.enabled = state;
        }
    }
    void SetParticles( bool state )
    {
        if( particles == null || particles.Length == 0 ) { return; }

        foreach( ParticleSystem particle in particles ) {
            if( state ) {
                particle.Play();
            } else {
                particle.Stop();
            }
        }
    }

    private void OnTriggerEnter( Collider other )
    {
        CheckForFlameObject( other );
    }
    private void OnTriggerStay( Collider other )
    {
        CheckForController( other );
    }

    void CheckForController( Collider other )
    {
        VRTK_ControllerEvents events = other.GetComponentInParent<VRTK_ControllerEvents>();
        if( events == null ) { return; }
        
        if( events.GetTriggerAxis() >= events.triggerClickThreshold ) {
            if( isLit && isExtinguishable ) {
                IsLit = false;
            }
        }
    }
    void CheckForFlameObject( Collider other )
    {
        if( !other.isTrigger ) { return; }

        FlameObject flameObject = other.GetComponentInParent<FlameObject>();
        if( flameObject == null ) { return; }

        if( !isLit || flameObject.IsLit || !flameObject.isLightable || !canTransferFlame ) {
            return;
        }

        flameObject.IsLit = true;
    }
}
