using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tweener<T>
{
    public bool spherical = false;
    public bool active = false;

    protected T startValue;
    protected T targetValue;
    protected abstract T changeValue { get; }

    protected float startTime;
    protected float duration;

    public Tweener( bool in_spherical )
    {
        spherical = in_spherical;
    }

    public bool StartTween( T start, T target, float in_duration, float equalTargetRange = 0.1f )
    {
        if( equalTargetRange >= 0 && IsSameTarget( target, equalTargetRange ) ) {
            return false;
        }

        startValue = start;
        targetValue = target;
        duration = in_duration;
        startTime = Time.time;

        return active = true;
    }
    protected abstract bool IsSameTarget( T target, float equalTargetRange );

    public T Update( T atVal )
    {
        if( active ) {
            if( Time.time < startTime + duration ) {
                if( spherical ) {
                    return SphericalTween();
                }
                return LinearTween();
            } else {
                active = false;
                return targetValue;
            }
        }

        return atVal;
    }

    public T SphericalTween()
    {
        T value;
        float time = Time.time - startTime;

        value = SphericalCalc( time );
        return value;
    }
    protected abstract T SphericalCalc( float time );

    public T LinearTween()
    {
        T value;
        float time = Time.time - startTime;

        value = LinearCalc( time );
        return value;
    }
    protected abstract T LinearCalc( float time );
}

public class DoubleTweener : Tweener<double>
{
    public DoubleTweener( bool in_spherical )
        : base( in_spherical ) { }

    protected override double changeValue
    {
        get {
            return targetValue - startValue;
        }
    }

    protected override bool IsSameTarget( double target, float equalTargetRange )
    {
        return ( Math.Abs(targetValue - target) <= equalTargetRange );
    }
    protected override double SphericalCalc( float time )
    {
        return -changeValue / 2 * ( Mathf.Cos( Mathf.PI * time / duration ) - 1 ) + startValue;
    }
    protected override double LinearCalc( float time )
    {
        return changeValue * ( time / duration ) + startValue;
    }
}
public class Vector3Tweener : Tweener<Vector3>
{
    public Vector3Tweener( bool in_spherical )
        : base( in_spherical ) { }

    protected override Vector3 changeValue
    {
        get {
            return targetValue - startValue;
        }
    }

    protected override bool IsSameTarget( Vector3 target, float equalTargetRange )
    {
        return (targetValue - target).magnitude <= equalTargetRange;
    }
    protected override Vector3 SphericalCalc( float time )
    {
        return -changeValue / 2 * ( Mathf.Cos( Mathf.PI * time / duration ) - 1 ) + startValue;
    }
    protected override Vector3 LinearCalc( float time )
    {
        return changeValue * ( time / duration ) + startValue;
    }
}
public class FloatTweener : Tweener<float>
{
    public FloatTweener( bool in_spherical )
        : base( in_spherical ) { }

    protected override float changeValue
    {
        get {
            return targetValue - startValue;
        }
    }

    protected override bool IsSameTarget( float target, float equalTargetRange )
    {
        return ( Math.Abs( targetValue - target ) <= equalTargetRange );
    }
    protected override float SphericalCalc( float time )
    {
        return -changeValue / 2 * ( Mathf.Cos( Mathf.PI * time / duration ) - 1 ) + startValue;
    }
    protected override float LinearCalc( float time )
    {
        return changeValue * ( time / duration ) + startValue;
    }
}