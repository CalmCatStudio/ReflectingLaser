using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootReflectingLaser : MonoBehaviour
{
    [SerializeField]
    private Transform laserOrigin = null;
    [SerializeField]
    private LineRenderer line = null;
    private Vector3[] points = null;
    [SerializeField, Range(0, 100)]
    private int bounceTotal = 5;
    [SerializeField, Range(0f, 100f)]
    private float segmentLength = 10f;
    [SerializeField]
    private bool shortRangeLastBounce = true;

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
        // TODO: Implement optional sway boolean(IE: An aim wobble while sniping.)

        points = reflecting.GetReflectingPoints(laserOrigin.position, transform.up, points, shortRangeLastBounce, segmentLength);        
        line.SetPositions(points);
    }

    private void SetupLineRenderer()
    {
        if (line == null)
        {
            Debug.LogError("Line Renderer is null. Attempting to find one", gameObject);
            if (!TryGetComponent(out line))
            {
                line = GetComponentInChildren<LineRenderer>();
            }
        }

        // We need a point for the origin, and the point after the last bounce; So bounceTotal + 2.
        int totalPoints = bounceTotal + 2;
        // Cache the lines we will need.
        line.positionCount = totalPoints;
        // Initialize the array we need to hold the points.
        points = new Vector3[totalPoints];
    }
}
