using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Aspect
{
    Circle,
    Ring,
    Circle_Runes,
    Triangle,
    Wedge
}
public enum Sigil
{
    Fire,
    Earth,
    Water,
    Air,
    Candle,
    Hearth,
    Crystal
}

public static class AspectResources
{
    public static Material GetComponentMaterial( Aspect aspect )
    {
        Material material;

        switch( aspect ) {
            case Aspect.Circle:
                material = Resources.Load<Material>( "Materials/Aspects/Circle_Aspect" );
                break;
            case Aspect.Ring:
                material = Resources.Load<Material>( "Materials/Aspects/Ring_Aspect" );
                break;
            case Aspect.Circle_Runes:
                material = Resources.Load<Material>( "Materials/Aspects/Circle_Runes" );
                break;
            case Aspect.Triangle:
                material = Resources.Load<Material>( "Materials/Aspects/Triangle_Aspect" );
                break;
            case Aspect.Wedge:
                material = Resources.Load<Material>( "Materials/Aspects/Wedge_Aspect" );
                break;
            default:
                material = new Material( Shader.Find( "Incant/Seal" ) )
                {
                    color = new Color( 1, 1, 1, 0 )
                };
                break;
        }
        
        return material;
    }
    public static Material GetComponentMaterial( Sigil sigil )
    {
        Material material;

        switch( sigil ) {
            case Sigil.Fire:
                material = Resources.Load<Material>( "Materials/Sigils/Fire_Sigil" );
                break;
            case Sigil.Earth:
                material = Resources.Load<Material>( "Materials/Sigils/Earth_Sigil" );
                break;
            case Sigil.Water:
                material = Resources.Load<Material>( "Materials/Sigils/Water_Sigil" );
                break;
            case Sigil.Air:
                material = Resources.Load<Material>( "Materials/Sigils/Air_Sigil" );
                break;
            case Sigil.Candle:
                material = Resources.Load<Material>( "Materials/Sigils/Candle_Sigil" );
                break;
            case Sigil.Hearth:
                material = Resources.Load<Material>( "Materials/Sigils/Hearth_Sigil" );
                break;
            case Sigil.Crystal:
                material = Resources.Load<Material>( "Materials/Sigils/Crystal_Sigil" );
                break;
            default:
                material = new Material( Shader.Find( "Incant/Seal" ) )
                {
                    color = new Color( 1, 1, 1, 0 )
                };
                break;
        }

        return material;
    }
}
