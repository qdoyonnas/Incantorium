using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SealComponent : MonoBehaviour
{
    #region Public Fields
    public enum SpinDirection {
        COUNTERCLOCKWISE,
        EITHER,
        CLOCKWISE
    };

    public Vector3 pivotPoint = Vector3.zero;
    #endregion

    Material aspectMaterial;
    float spinDegrees = 0;
    float orbitDegrees = 0;
    Vector3 spinAxis = Vector3.up;

    float RollSpin( SpinDirection direction, float min, float max )
    {
        float degrees = 0;

        if( min > max ) {
            min = max;
        }

        switch( direction ) {
            case SpinDirection.COUNTERCLOCKWISE:
                degrees = Random.Range( -max, -min );
                break;
            case SpinDirection.EITHER:
                degrees = Random.Range( -max, max );
                while( Mathf.Abs( degrees ) < min ) {
                    degrees = Random.Range( -max, max );
                }
                break;
            case SpinDirection.CLOCKWISE:
                degrees = Random.Range( min, max );
                break;
        }

        return degrees;
    }

    public float Spin( float degrees )
    {
        spinDegrees = degrees;

        return spinDegrees;
    }
    public float Spin( SpinDirection direction, float min, float max )
    {
        spinDegrees = RollSpin( direction, min, max );

        return spinDegrees;
    }
    
    public float Orbit( Vector3 offsetPosition, Vector3 pivot, float degrees )
    {
        transform.localPosition = offsetPosition;
        pivotPoint = pivot;
        orbitDegrees = degrees;

        return degrees;
    }
    public float Orbit( Vector3 offsetPosition, Vector3 pivot, SpinDirection direction, float min, float max )
    {
        transform.localPosition = offsetPosition;
        pivotPoint = pivot;

        orbitDegrees = RollSpin( direction, min, max );

        return orbitDegrees;
    }

    void InitObject()
    {
        Destroy( gameObject.GetComponent<Collider>() );
        transform.localScale = Vector3.one;
        gameObject.GetComponent<Renderer>().material = aspectMaterial;
        gameObject.layer = transform.parent.gameObject.layer;
    }
    public void Init( Material mat )
    {
        aspectMaterial = new Material( mat );
        InitObject();

        Color createColor = aspectMaterial.color;
        createColor.a = 0;
        aspectMaterial.DOColor( createColor, 1 ).From();
    }
	
	void FixedUpdate () {
        transform.Rotate( spinAxis, spinDegrees );
        transform.RotateAround( GetPivot(), transform.TransformDirection(spinAxis), orbitDegrees );
	}

    public void DoDestroy()
    {
        if( aspectMaterial == null || gameObject == null ) { return; }

        Color destroyColor = aspectMaterial.color;
        destroyColor.a = 0;
        aspectMaterial.DOColor( destroyColor, 1 )
            .OnComplete( () => Destroy(gameObject) );
    }
    public void DoDestroy( Sequence destroySequence )
    {
        if( aspectMaterial == null || gameObject == null ) { return; }

        Color destroyColor = aspectMaterial.color;
        destroyColor.a = 0;
        destroySequence.Insert( 0, aspectMaterial.DOColor( destroyColor, 1 ) );
    }

    private void OnDestroy()
    {
        Destroy( aspectMaterial );
    }

    Vector3 GetPivot()
    {
        return transform.parent.position + pivotPoint;
    }
}
