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
    // TODO: Bounce total, and segmentLength make judging distance unintuitive. The range doesn't change as you get closer, and further from the wall; UNLESS the segment changes.
    // I think a total distance would work, but knowing the size of the points array seems impossible. Would need to use a list, and convert it to array for the Line Renderer.
    //[SerializeField, Range(0, 100)]
    //private int bounceTotal = 5;
    //[SerializeField, Range(0f, 100f)]
    //private float segmentLength = 10f;
    //[SerializeField]
    //private bool shortRangeLastBounce = true;
    [SerializeField, Min(0)]
    private float maxRange = 100f;
    [SerializeField, Min(0)]
    private int maxBounces = 10;

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

        points = reflecting.GetReflectingPoints(laserOrigin.position, transform.up, maxRange, points);        
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
        int totalPoints = maxBounces + 2;
        // Cache the lines we will need.
        line.positionCount = totalPoints;
        // Initialize the array we need to hold the points.
        points = new Vector3[totalPoints];
    }
}
