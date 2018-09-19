using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicObject : MonoBehaviour
{
    public Sigil[] initialSigils;
    public bool isOwned = true;
    public float sealScale = 0.1f;

    Transform anchor;
    Magic magicScript;
    Seal seal;
    List<Sigil> sigils = new List<Sigil>();

    public void AddSigil( Sigil sigil )
    {
        if( sigils.Contains( sigil ) ) { return; }

        sigils.Add( sigil );
    }
    public void RemoveSigil( Sigil sigil )
    {
        sigils.Remove( sigil );
    }

    private void Start()
    {
        Transform anchorTransform = transform.Find( "SealAnchor" );
        anchor = (anchorTransform != null ? anchorTransform : transform);

        magicScript = VRTKScripts.instance.GetComponentInChildren<Magic>();
        if( magicScript != null ) {
            magicScript.OnFocus += DoOnFocus;
            magicScript.OnUnfocus += DoOnUnfocus;
        }

        foreach( Sigil sigil in initialSigils ) {
            sigils.Add( sigil );
        }
    }

    public Seal DisplaySeal( bool focused = false )
    {
        if( !isOwned || seal != null ) { return null; }

        GameObject sealPrefab = Resources.Load<GameObject>( "Prefabs/Seal" );
        GameObject anchorObj = GameObject.Instantiate<GameObject>( sealPrefab, anchor.position, Quaternion.identity );
        anchorObj.transform.localScale = new Vector3( sealScale, sealScale, sealScale );
        seal = anchorObj.GetComponent<Seal>();
        if( !seal.Init( sigils.ToArray() ) ) {
            Destroy( seal );
            return null;
        }

        if( focused ) {
            seal.Focus();
        }

        if( sigils.Count > 0 ) {
            seal.CreateCoreSigil( AspectResources.GetComponentMaterial( sigils[0] ) );
            for( int i = 1; i < sigils.Count; i++ ) {
                seal.CreateOrbitSigil( AspectResources.GetComponentMaterial( sigils[i] ) );
            }
        }

        return seal;
    }
    void DoOnFocus()
    {
        if( !isOwned ) { return; }

        //DisplaySeal();
    }
    void DoOnUnfocus()
    {
        if( !isOwned || seal == null ) { return; }
        seal.DoDestroy();
    }
}
