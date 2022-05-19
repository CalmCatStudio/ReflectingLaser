using System;
using UnityEngine;

public class ReflectingPoints
{
    /// <summary>
    /// Uses raycasts to determine an array of points to represent a path that is reflecting of walls.
    /// </summary>
    /// <param name="origin">Start location</param>
    /// <param name="direction">Direction to fire</param>
    /// <param name="range">The range of the laser</param>
    /// <param name="pointsContainer">The array that will hold the points. Also determines the max number of bounces.</param>
    /// <returns>A Vector3 array containing points representing a path that reflects off wall.</returns>
    public Vector3[] GetReflectingPoints(Vector3 origin, Vector3 direction, float range, Vector3[] pointsContainer)
    {
        int maxBounces = pointsContainer.Length - 1;
        pointsContainer[0] = origin;
        Vector3 point = origin;
        var ray = new ReflectingRayInfo();

        int i = 1;
        while (i <= maxBounces)
        {
            // Check if there is range to keep going.
            if (range <= 0)
            {
                // If there isn't then every point left will end at this location. This allows the line to finish itself.
                pointsContainer[i] = point;
                i++;
                continue;
            }

            ray = FireReflectingShot(point, direction, range);
            direction = ray.direction;

            // Find the distance between the old point, and the new one, and subract it from the range.
            float distance = Vector3.Distance(point, ray.point);
            range -= distance;

            // Set the old as the new point
            point = ray.point;
            // Increment i after adding the point into the container.
            pointsContainer[i++] = point;
        }

        return pointsContainer;
    }

    private ReflectingRayInfo FireReflectingShot(Vector3 point, Vector3 direction, float range)
    {
        RaycastHit2D hit = Physics2D.Raycast(point, direction, range);
        var reflectingRay = new ReflectingRayInfo();
        if (!hit)
        {
            reflectingRay.point = point + (direction * range);
            reflectingRay.direction = direction;
        }
        else
        {
            reflectingRay.point = hit.point;
            reflectingRay.direction = Vector2.Reflect(direction, hit.normal);
        }
        return reflectingRay;
    }

    [Obsolete("Use overload with float range instead. The distance traveled makes more sense.")]
    /// <summary>
    /// Uses raycasts to determine an array of points to represent a path that is reflecting of walls.
    /// </summary>
    /// <param name="origin">Starting location</param>
    /// <param name="direction">Direction to fire</param>
    /// <param name="pointsContainer">The size of this container determines the number of bounces: Length - 2 Bounces(The origin and end point explain the -2)</param>
    /// <param name="shortRangeLastShot">Whether the last shot should reach the next wall or not</param>
    /// <param name="range">The range of each shot</param>
    /// <returns>An array of points representing a path that reflects off walls</returns>
    public Vector3[] GetReflectingPoints(Vector3 origin, Vector3 direction, Vector3[] pointsContainer, bool shortRangeLastShot = false, float range = 10f)
    {
        pointsContainer[0] = origin;
        Vector3 point = origin;

        int count = pointsContainer.Length;
        for (int i = 1; i < count; i++)
        {
            // Last shot AND a shorter range is desired.
            if (shortRangeLastShot && i == count - 1)
            {
                // Decrease the range for the last shot.
                range *= .5f;
            }
            var hit = Physics2D.Raycast(point, direction, range);
            if (!hit)
            {
                // A missed hit should go a small amount in the direction aimed.
                point = point + (direction * (range * 0.5f));
            }
            else
            {
                point = hit.point;
                direction = Vector2.Reflect(direction, hit.normal);
            }
            pointsContainer[i] = point;
        }
        return pointsContainer;
    }

    [Obsolete("Use overload with float range instead. The distance traveled makes more sense.")]
    /// <summary>
    /// Uses raycasts to determine an array of points to represent a path that is reflecting of walls.
    /// </summary>
    /// <param name="origin">Starting location</param>
    /// <param name="direction">Direction to fire</param>
    /// <param name="numOfBounces">The number of bounces before stopping. Note one last shot will be fired after the last bounce</param>
    /// <param name="shortRangeLastShot">Whether the last shot should reach the next wall or not</param>
    /// <param name="range">The range of each shot</param>
    /// <returns>An array of points representing a path that reflects off walls</returns>
    public Vector3[] GetReflectingPoints(Vector3 origin, Vector3 direction, int numOfBounces, bool shortRangeLastShot = false, float range = 10f)
    {
        // Origin, and last point need to be included; So numOfBounces + 2;
        int totalPoints = numOfBounces + 2;
        var points = new Vector3[totalPoints];
        points[0] = origin;
        Vector3 point = origin;
        for (int i = 1; i < totalPoints; i++)
        {
            if (shortRangeLastShot && i == totalPoints - 1)
            {
                // Decrease the range for the last shot.
                range = range * .5f;
            }
            var hit = Physics2D.Raycast(point, direction, range);
            if (!hit)
            {
                // A missed hit should go a small amount in the direction aimed.
                point = point + (direction * (range * 0.5f));
            }
            else
            {
                point = hit.point;
                direction = Vector2.Reflect(direction, hit.normal);
            }
            points[i] = point;
        }
        return points;
    }
}

public struct ReflectingRayInfo
{
    public Vector3 point;
    public Vector3 direction;
}
