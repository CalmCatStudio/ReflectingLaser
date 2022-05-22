using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Extremely simple controller for debugging laser issues.
/// </summary>
public class SimpleTwinstickMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField, Range(0, 500)]
    private float rotationSpeed = 10f;
    private float desiredRotationChange = 0f;

    private Rigidbody2D rb = null;

    private void Awake()
    {
        if (!TryGetComponent(out rb))
        {
            Debug.LogError("No Rigidbody found on Floating Diamond", gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity * moveSpeed * Time.deltaTime;        
        rb.MoveRotation(rb.rotation + (desiredRotationChange * rotationSpeed * Time.deltaTime));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        velocity = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        desiredRotationChange = -context.ReadValue<Vector2>().x;
    }
}
