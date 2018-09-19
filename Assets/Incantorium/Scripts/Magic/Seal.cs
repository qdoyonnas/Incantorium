using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using VRTK;

public class Seal : MonoBehaviour
{
    #region Static

    public const float TravelTime = 0.5f;
    public const float OrbitingScaleFactor = 1.5f;
    public const float OrbitDistance = (1f * 5f);
    public const float ActivateDistance = 3f;
    public const float Friction = 0.05f;

    #endregion

    bool alive = true;
    bool focused = false;
    List<Sigil> sigils = new List<Sigil>();
    Dictionary<string, SealComponent> sealComponents = new Dictionary<string, SealComponent>();
    float baseScale = 1f;

    Vector3 velocity = Vector3.zero;
    float trackRotation = 0;

    public bool Init( Sigil[] in_sigils )
    {
        Transform playTransform = VRTK_DeviceFinder.PlayAreaTransform();
        float distanceToPlayArea = Vector3.Distance( transform.position, playTransform.position );

        if( distanceToPlayArea > ActivateDistance ) {
            return false;
        }

        baseScale = transform.localScale.x;

        foreach( Sigil sigil in in_sigils ) {
            sigils.Add( sigil );
        }

        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 5f;

        sealComponents.Clear();

        CreateCenterAspect( AspectResources.GetComponentMaterial(Aspect.Ring) );
        CreateCenterAspect( AspectResources.GetComponentMaterial(Aspect.Triangle) );

        return true;
    }

    bool CheckTriggers()
    {
        bool state = false;

        SphereCollider collider = GetComponent<SphereCollider>();
        GameObject[] magicObjects = GameObject.FindGameObjectsWithTag( "MagicObject" );

        foreach( GameObject obj in magicObjects ) {

            if( collider.bounds.Contains( obj.transform.position ) ) {
                if( sigils.Contains( Sigil.Fire ) ) {
                    FlameObject flameScript = obj.GetComponent<FlameObject>();
                    if( flameScript != null && flameScript.IsLit == false ) {
                        flameScript.IsLit = true;
                        state = true;
                    }
                }

                if( sigils.Contains( Sigil.Earth ) ) {
                    DoorTrigger doorScript = obj.GetComponent<DoorTrigger>();
                    if( doorScript != null ) {
                        doorScript.Trigger();
                    }
                }
            }

        }

        return state;
    }
    public void DoDestroy()
    {
        alive = false;

        CheckTriggers();

        Sequence destroySequence = DOTween.Sequence();

        foreach( SealComponent component in sealComponents.Values ) {
            component.DoDestroy( destroySequence );
        }

        destroySequence.AppendInterval( 0.05f );
        destroySequence.AppendCallback( () => Destroy(gameObject) );
    }

    public void RemoveComponent( string name )
    {
        sealComponents.Remove( name );
    }

    public void CreateCenterAspect( Material mat )
    {
        SealComponent component = CreateComponent( "CenterAspect_" + mat.name, mat );
        component.Spin( SealComponent.SpinDirection.EITHER, 0.1f, 1f );
    }
    public float CreateOrbitAspect( string sigilName, Vector3 orbitPosition, Material mat )
    {
        SealComponent component = CreateComponent( sigilName + "_OrbitAspect_" + mat.name ,mat );
        component.transform.localScale = Vector3.one / (1.68f * OrbitingScaleFactor);
        component.Spin( SealComponent.SpinDirection.EITHER, 0.1f, 1f );
        float orbitDegrees = component.Orbit( orbitPosition, Vector3.zero, SealComponent.SpinDirection.EITHER, 0.1f, 1f );

        return orbitDegrees;
    }
    public void CreateOrbitSigil( Material mat )
    {
        float offsetDegrees = Random.Range( 0, Mathf.PI * 2 );
        Vector3 orbitPosition = new Vector3( Mathf.Cos( offsetDegrees ), Mathf.Sin( offsetDegrees ), 0 ) * OrbitDistance;

        float orbitDegrees = CreateOrbitAspect( mat.name, orbitPosition, AspectResources.GetComponentMaterial( Aspect.Circle ) );

        SealComponent component = CreateComponent( "OrbitSigil_" + mat.name, mat );
        component.transform.Rotate( Vector3.up, (Mathf.Rad2Deg * offsetDegrees) - 90 );
        component.transform.localScale = Vector3.one / (1.68f * OrbitingScaleFactor);

        component.Orbit( orbitPosition, Vector3.zero, orbitDegrees );
    }
    public void CreateCoreSigil( Material mat )
    {
        CreateComponent( "CoreSigil", mat );
        CreateOrbitSigil( mat );
    }
    public SealComponent CreateComponent( string name, Material mat )
    {
        if( sealComponents.ContainsKey( name ) ) {
            sealComponents[name].DoDestroy();
        }

        GameObject obj = GameObject.CreatePrimitive( PrimitiveType.Plane );
        obj.name = name;
        obj.transform.parent = this.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = new Vector3( 90, 0, 0 );

        SealComponent component = obj.AddComponent<SealComponent>();
        component.Init( mat );

        sealComponents[name] = component;

        return component;
    }

    public void Focus()
    {
        if( focused ) { return; }

        focused = true;
        SealComponent component = CreateComponent( "Focus", AspectResources.GetComponentMaterial(Aspect.Circle_Runes) );
        component.Spin( SealComponent.SpinDirection.EITHER, 0.1f, 1f );
    }
    public void Unfocus()
    {
        if( !focused ) { return; }

        focused = false;
        RemoveComponent( "Focus" );
    }

    void CheckDistance()
    {
        Transform playTransform = VRTK_DeviceFinder.PlayAreaTransform();
        float distanceToPlayArea = Vector3.Distance( transform.position, playTransform.position );

        if( distanceToPlayArea > ActivateDistance ) {
            DoDestroy();
            return;
        }

        float relativeScale = baseScale * (distanceToPlayArea / ActivateDistance);
        relativeScale = (relativeScale < 0 ? 0 : relativeScale);
        transform.localScale = Vector3.one * relativeScale;
    }

    public void RunInputs( Vector3 positionDelta, Vector3 rotationDelta )
    {
        if( !alive ) { return; }

        CheckDistance();

        velocity += positionDelta * 0.4f;
        if( velocity.magnitude > 2f ) {
            velocity = velocity.normalized * 2f;
        }
    }

    private void FixedUpdate()
    {
        transform.position += velocity;
        velocity -= velocity * Friction;
        if( velocity.magnitude < 0.01f ) {
            velocity = Vector3.zero;
        }

        if( CheckTriggers() ) {
            DoDestroy();
        }
    }
}
