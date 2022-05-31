using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget2D : TestTarget
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private Color
        baseColor = Color.blue,
        hitColor = Color.green;

    private void Update()
    {
        EvaluateHit();
        hitThisFrame = false;
    }

    private void EvaluateHit()
    {
        if (!hitThisFrame)
        {
            spriteRenderer.color = baseColor;
        }
        else
        {
            spriteRenderer.color = hitColor;
        }
    }
}
