using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate2D : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 10f;
    private Rigidbody2D rb = null;
    [SerializeField, Range(-1, 1), Tooltip("-1 for left, 1 for right")]
    private int direction = 1;

    private void Awake()
    {
        if (!TryGetComponent(out rb))
        {
            Debug.LogError("Gameobject missing rigidbody", gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(rb.rotation + ((direction * rotationSpeed) * Time.deltaTime));
    }
}
