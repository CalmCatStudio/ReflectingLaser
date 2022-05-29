using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget2D : MonoBehaviour, ILaserTarget
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private Color
        baseColor,
        hitColor;

    private bool hitThisFrame = false;

    private void Update()
    {
        if (!hitThisFrame)
        {
            spriteRenderer.color = baseColor;
        }
        else
        {
            spriteRenderer.color = hitColor;
        }

        hitThisFrame = false;
    }

    public void HitByLaser()
    {
        print("Hit");
        hitThisFrame = true;
    }


}
