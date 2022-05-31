using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget3D : TestTarget
{
    [SerializeField]
    private Material material;
    [SerializeField]
    private Color
        baseColor,
        hitColor;

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
}
