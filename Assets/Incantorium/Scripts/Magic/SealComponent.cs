using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealComponent : MonoBehaviour
{
    public enum ComponentType
    {
        CIRCLE,
        RING,
        CIRCLERUNES,
        TRIANGLE,
        WEDGE
    }

    #region Public Fields
    public enum SpinDirection {
        COUNTERCLOCKWISE,
        EITHER,
        CLOCKWISE
    };

    public Vector3 pivotPoint = Vector3.zero;
    public SpinDirection spinDirection = SpinDirection.EITHER;
    public float minSpinRate = 0;
    public float maxSpinRate = 10;
    #endregion

    Material aspectMaterial;
    float spinDegrees = 0;
    Vector3 spinAxis = Vector3.up;

	public void Init( ComponentType type, Vector2 pivot, SpinDirection direction, Vector3 axis, float minSpin, float maxSpin )
    {
        InitObject( type );
        SetFields( pivot, direction, axis, minSpin, maxSpin );
        InitSpin();
    }
    void InitObject( ComponentType type )
    {
        Destroy( gameObject.GetComponent<Collider>() );
        transform.localScale = Vector3.one;

        switch( type ) {
            case SealComponent.ComponentType.CIRCLE:
                gameObject.name = "Circle";
                aspectMaterial = Resources.Load<Material>( "Aspects/Circle_Aspect" );
                gameObject.GetComponent<Renderer>().material = aspectMaterial;
                break;
            case SealComponent.ComponentType.RING:
                gameObject.name = "Ring";
                aspectMaterial = Resources.Load<Material>( "Aspects/Ring_Aspect" );
                gameObject.GetComponent<Renderer>().material = aspectMaterial;
                break;
            case SealComponent.ComponentType.CIRCLERUNES:
                gameObject.name = "CircleRunes";
                aspectMaterial = Resources.Load<Material>( "Aspects/Circle_Runes" );
                gameObject.GetComponent<Renderer>().material = aspectMaterial;
                break;
            case SealComponent.ComponentType.TRIANGLE:
                gameObject.name = "Triangle";
                aspectMaterial = Resources.Load<Material>( "Aspects/Triangle_Aspect" );
                gameObject.GetComponent<Renderer>().material = aspectMaterial;
                break;
            case SealComponent.ComponentType.WEDGE:
                gameObject.name = "Wedge";
                aspectMaterial = Resources.Load<Material>( "Aspects/Wedge_Aspect" );
                gameObject.GetComponent<Renderer>().material = aspectMaterial;
                break;
        }
    }
    void SetFields( Vector2 pivot, SpinDirection direction, Vector3 axis, float minSpin, float maxSpin )
    {
        pivotPoint = pivot;
        spinDirection = direction;
        spinAxis = axis;
        minSpinRate = minSpin;
        maxSpinRate = maxSpin;
    }
    void InitSpin()
    {
        if( minSpinRate > maxSpinRate ) {
            minSpinRate = maxSpinRate;
        }

        switch( spinDirection ) {
            case SpinDirection.COUNTERCLOCKWISE:
                spinDegrees = Random.Range( -maxSpinRate, -minSpinRate );
                break;
            case SpinDirection.EITHER:
                spinDegrees = Random.Range( -maxSpinRate, maxSpinRate );
                while( Mathf.Abs(spinDegrees) < minSpinRate ) {
                    spinDegrees = Random.Range( -maxSpinRate, maxSpinRate );
                }
                break;
            case SpinDirection.CLOCKWISE:
                Random.Range( minSpinRate, maxSpinRate );
                break;
        }
    }
	
	void FixedUpdate () {
        transform.RotateAround( GetPivot(), transform.TransformDirection(spinAxis) , spinDegrees );
	}

    private void OnDestroy()
    {
        //Destroy( aspectMaterial );
    }

    Vector3 GetPivot()
    {
        return transform.position += pivotPoint;
    }
}
