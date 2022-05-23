using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootReflectingLaser : MonoBehaviour
{
    [SerializeField]
    private bool use2DPhysics = true;
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
    [SerializeField]
    private LayerMask layersToBounceOff = default;

    private Vector3 aimDirection = Vector3.zero;

    private ReflectingPoints reflecting = new ReflectingPoints();

    private void OnValidate()
    {
        // Allows changing the bounce total in the editor.
        SetupLineRenderer();
    }

    private void Awake()
    {
        SetupLineRenderer();
    }

    private void Update()
    {
        // TODO: Implement optional sway(IE: An aim wobble while sniping.)
        // Technically we could only fire this when this object has moved, but then if the world changes it wouldn't update properly.
        aimDirection = transform.up;
        points = reflecting.GetReflectingPoints(origin.position, aimDirection, maxRange, layersToBounceOff, points, use2DPhysics);        
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
