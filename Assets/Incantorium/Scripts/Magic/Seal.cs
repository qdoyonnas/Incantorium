using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seal : MonoBehaviour
{
    #region Static

    public const float TravelTime = 0.5f;

    #endregion

    #region Public Fields

    public Vector3 target;

    #endregion

    #region Private Fields

    Vector3Tweener positionTween = new Vector3Tweener( true );
    Vector3 velocity;
    bool _isActive = false;
    List<SealComponent> sealComponents = new List<SealComponent>();

    #endregion

    #region Properties

    public bool isActive { get; set; }

    #endregion

    #region Create Methods

    public void Create( SealControls sealControls )
    {
        transform.parent.position = sealControls.transform.position;

        sealComponents.Add( CreateSpellComponent( SealComponent.ComponentType.RING ) );
        sealComponents.Add( CreateSpellComponent( SealComponent.ComponentType.CIRCLERUNES ) );
        sealComponents.Add( CreateSpellComponent( SealComponent.ComponentType.TRIANGLE ) );

        isActive = true;
    }
    SealComponent CreateSpellComponent( SealComponent.ComponentType type )
    {
        GameObject obj = GameObject.CreatePrimitive( PrimitiveType.Plane );
        obj.transform.parent = this.transform;
        obj.transform.localPosition = Vector3.zero;

        SealComponent component = obj.AddComponent<SealComponent>();
        component.Init( type, Vector3.zero, SealComponent.SpinDirection.EITHER, Vector3.up, 0.2f, 1.5f );
        
        return component;
    }

    #endregion

    public void MoveTo( Vector3 in_target )
    {
        positionTween.StartTween( transform.parent.position, in_target, TravelTime );
    }

    #region Unity Methods

    public void SetActive( bool state )
    {
        transform.parent.gameObject.SetActive( state );
    }

    void Update ()
    {
        transform.parent.position = positionTween.Update( transform.parent.position );
    }

    #endregion
}
