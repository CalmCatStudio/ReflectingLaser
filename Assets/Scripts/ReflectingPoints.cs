using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingPoints
{
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

    /// <summary>
    /// Uses raycasts to determine an array of points to represent a path that is reflecting of walls.
    /// Less performant than passing the array.
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
