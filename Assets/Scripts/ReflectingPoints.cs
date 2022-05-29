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
    public Vector3[] GetReflectingPoints(Vector3 origin, Vector3 direction, float range, LayerMask layersToBounceOff, Vector3[] pointsContainer, bool use2DPhysics = true, bool notifyTarget = false)
    {
        int maxBounces = pointsContainer.Length;
        pointsContainer[0] = origin;
        Vector3 point = origin;

        var reflectingRay = new ReflectingRayInfo();
        bool hitObstacle = false;
        int i = 1;
        while (i < maxBounces)
        {
            // Check if line should keep going.
            if (range < 0 || hitObstacle == true)
            {
                // Every point left will end at this location. This allows the line to finish itself.
                pointsContainer[i] = point;
                i++;
                continue;
            }

            if (use2DPhysics)
            {
                reflectingRay = FireReflectingShot2D(reflectingRay, point, direction, range, layersToBounceOff, notifyTarget);
            }
            else
            {
                reflectingRay = FireReflectingShot3D(reflectingRay, point, direction, range, layersToBounceOff, notifyTarget);
            }

            direction = reflectingRay.direction;
            hitObstacle = reflectingRay.hitNonBouncable;

            // Find the distance between the old point, and the new one, and subtract it from the range.
            float distance = Vector3.Distance(point, reflectingRay.point);
            range -= distance;

            // Set the point to the new location.
            point = reflectingRay.point;
            // Increment i after adding the point into the container.
            pointsContainer[i++] = point;
        }

        return pointsContainer;
    }

    private ReflectingRayInfo FireReflectingShot2D(ReflectingRayInfo reflectingRay, Vector3 origin, Vector3 direction, float range, LayerMask bouncableLayers, bool notifyTarget)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, range);
        reflectingRay.hitNonBouncable = false;
        // Ray did not hit anything.
        if (!hit)
        {
            // Create a point at the max range a single segment can go.
            reflectingRay.point = origin + (direction * range);
            reflectingRay.direction = direction;
        }
        else
        {
            // Hit a non bouncable object.
            if (!IsLayerBouncable(hit.collider.gameObject.layer, bouncableLayers))
            {
                reflectingRay.hitNonBouncable = true;
            }

            if (notifyTarget && hit.collider.TryGetComponent(out ILaserTarget target))
            {
                target.HitByLaser();
            }
            

            reflectingRay.point = hit.point;
            reflectingRay.direction = Vector2.Reflect(direction, hit.normal);
        }
        return reflectingRay;
    }

    private ReflectingRayInfo FireReflectingShot3D(ReflectingRayInfo reflectingRay, Vector3 origin, Vector3 direction, float range, LayerMask bouncableLayers, bool notifyTarget)
    {
        reflectingRay.hitNonBouncable = false;
        RaycastHit hit;
        // Fire a ray, If it misses go into this if statement; Otherwise place the results into hit.
        if (!Physics.Raycast(origin, direction, out hit, range))
        {
            // Create a point at the max range a single segment can go.
            reflectingRay.point = origin + (direction * range);
            reflectingRay.direction = direction;
        }
        else
        {
            // Hit a non bouncable object.
            if (!IsLayerBouncable(hit.collider.gameObject.layer, bouncableLayers))
            {
                reflectingRay.hitNonBouncable = true;
            }

            if (notifyTarget && hit.collider.TryGetComponent(out ILaserTarget target))
            {
                target.HitByLaser();
            }

            reflectingRay.point = hit.point;
            reflectingRay.direction = Vector3.Reflect(direction, hit.normal);
        }

        return reflectingRay;
    }

    private bool IsLayerBouncable(int layer, LayerMask bouncableLayers)
    {
        if (((1 << layer) & bouncableLayers) == 0)
        {
            return false;
        }
        return true;
    }
}

public struct ReflectingRayInfo
{
    public Vector3 point;
    public Vector3 direction;
    public bool hitNonBouncable;
}
