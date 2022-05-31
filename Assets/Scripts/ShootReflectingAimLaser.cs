using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootReflectingAimLaser : MonoBehaviour
{
    [SerializeField]
    private bool use2DPhysics = true;
    [SerializeField, Tooltip("This will affect performance. Note: HurtTarget and AlertTarget together will barely affect performance.")]
    private bool alertTarget = false;
    [SerializeField, Tooltip("This will affect performance. Note: HurtTarget and AlertTarget together will barely affect performance.")]
    private bool hurtTarget = false;
    [SerializeField]
    private Transform origin = null;
    [SerializeField, Tooltip("The laser fires up from this objects rotation(the transform.up of this object)")]
    private Transform aimController = null;
    [SerializeField]
    private LineRenderer line = null;
    private Vector3[] points = null;
    [SerializeField, Min(0)]
    private float maxRange = 100f;
    [SerializeField, Range(0, 250), Tooltip("Note: Too many bounces may affect performance.")]
    private int maxBounces = 10;
    [SerializeField, Tooltip("The laser will phase through anything not included in this layermask")]
    private LayerMask layersToContact = default;
    [SerializeField, Tooltip("The laser will bounce off anything in this layermask(Unless it is not in the layersToContact)")]
    private LayerMask layersToBounceOff = default;

    private Vector3 aimDirection = Vector3.zero;

    private ReflectingPoints reflecting = null;

    private void OnValidate()
    {
        // Allows changing the bounce total in the editor.
        SetupLineRenderer();
    }

    private void Awake()
    {
        reflecting = new ReflectingPoints(use2DPhysics);
        SetupLineRenderer();
    }

    private void Update()
    {
        // Technically we could only fire when this object has moved, but then if the world changes it wouldn't update properly.
        aimDirection = aimController.transform.up;
        points = reflecting.GetReflectingPoints(origin.position, aimDirection, maxRange, layersToContact, layersToBounceOff, points, alertTarget, hurtTarget);        
        line.SetPositions(points);
    }

    private void SetupLineRenderer()
    {
        if (!line)
        {
            Debug.LogError("Line Renderer is null. Attempting to find one", gameObject);
            if (!TryGetComponent(out line))
            {
                line = GetComponentInChildren<LineRenderer>();
            }
        }

        // We need a point for the origin, and the point after the last bounce; So bounceTotal + 2.
        int totalPoints = maxBounces + 2;
        // Cache the lines we will need.
        line.positionCount = totalPoints;
        // Initialize the array we need to hold the points.
        points = new Vector3[totalPoints];
    }
}
