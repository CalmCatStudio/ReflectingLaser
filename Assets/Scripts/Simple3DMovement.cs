using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Extremely simple controller for debugging laser issues.
/// </summary>
public class Simple3DMovement : MonoBehaviour
{
    //[SerializeField]
    //private float moveSpeed = 10f;
    //private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private float rotationSpeed = 10f;
    private Vector3 desiredRotationChange = Vector3.zero;

    private Rigidbody rb = null;

    private void Awake()
    {
        if (!TryGetComponent(out rb))
        {
            Debug.LogError("No Rigidbody found on Laser", gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (desiredRotationChange != Vector3.zero)
        {
            Quaternion deltaRotation = Quaternion.Euler(desiredRotationChange);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Movement not implemented
        //velocity = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        desiredRotationChange = context.ReadValue<Vector2>();
        // Rotating the z works better for our purposes.
        desiredRotationChange.z = desiredRotationChange.y;
        desiredRotationChange.y = 0;
    }
}
