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

        int i = 1;
        while (i <= maxBounces)
        {
            if (range <= 0)
            {
                pointsContainer[i] = point;
                i++;
                continue;
            }

            var hit = Physics2D.Raycast(point, direction, range);
            Vector3 nextPoint = Vector3.zero;
            if (!hit)
            {
                point = point + (direction * range);
            }
            else
            {
                point = hit.point;
                direction = Vector2.Reflect(direction, hit.normal);
            }
            pointsContainer[i] = point;
            var distance = Vector3.Distance(point, nextPoint);
            range -= distance;
            i++;
        }

        return pointsContainer;
    }

    /// <summary>
    /// Deprecated.
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
    /// Deprecated.
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
