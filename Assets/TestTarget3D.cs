using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget3D : MonoBehaviour, ILaserTarget
{
    [SerializeField]
    private Material material;
    [SerializeField]
    private Color
        baseColor,
        hitColor;

    private bool hitThisFrame = false;

    private void Update()
    {
        if (!hitThisFrame)
        {
            material.color = baseColor;
        }
        else
        {
            material.color = hitColor;
        }

        hitThisFrame = false;
    }

    public void HitByLaser()
    {
        print("Hit");
        hitThisFrame = true;
    }
}
