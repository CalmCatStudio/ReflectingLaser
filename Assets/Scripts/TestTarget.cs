using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : MonoBehaviour, ILaserTarget
{
    protected bool hitThisFrame = false;
    public void TouchedByLaser(bool alert, bool hurt)
    {
        if (alert)
        {
            print("Hit");
            hitThisFrame = true;
        }

        if (hurt)
        {
            print("I don't get hurt by lasers");
        }
    }
}
