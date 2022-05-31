using System;
using UnityEngine;

public class ReflectingPoints
{
    private bool use2DPhysics;

    public ReflectingPoints(bool use2DPhysics)
    {
        this.use2DPhysics = use2DPhysics;
    }

    /// <summary>
    /// Uses raycasts to determine an array of points to represent a path that is reflecting of walls.
    /// </summary>
    /// <param name="rayOrigin">Start location</param>
    /// <param name="direction">Direction to fire</param>
    /// <param name="range">The range of the laser</param>
    /// <param name="pointsContainer">The array that will hold the points. Also determines the max number of bounces.</param>
    /// <returns>A Vector3 array containing points representing a path that reflects off wall.</returns>
    public Vector3[] GetReflectingPoints(Vector3 rayOrigin, Vector3 direction, float range, LayerMask layersToContact, LayerMask layersToBounceOff, Vector3[] pointsContainer, bool alertTarget = false, bool hurtTarget = false)
    {
        int maxBounces = pointsContainer.Length;
        Vector3 segmentOrigin = pointsContainer[0] = rayOrigin;
        var reflectingRay = new ReflectingRayInfo(rayOrigin, direction, alertTarget, hurtTarget);

        int i = 1;
        while (i < maxBounces)
        {
            // Check if line should keep going.
            if (range < 0 || reflectingRay.hitNonBouncable == true)
            {
                // If we quit without finishing the laser; Then it won't properly draw itself. To adhere to the Line Renderer settings every point must be set.
                // Every point left will end at this location. This allows the line to finish itself.
                pointsContainer[i] = segmentOrigin;
                i++;
                continue;
            }

            if (use2DPhysics)
            {
                FireReflectingShot2D(ref reflectingRay, segmentOrigin, range, layersToContact, layersToBounceOff);
            }
            else
            {
                FireReflectingShot3D(ref reflectingRay, segmentOrigin, range, layersToContact, layersToBounceOff);
            }

            // Find the distance between the old point, and the new one, and subtract it from the range.
            range -= Vector3.Distance(segmentOrigin, reflectingRay.segmentOrigin);

            // Set the new segmentOrigin
            segmentOrigin = reflectingRay.segmentOrigin;
            // Increment i after adding the point into the container.
            pointsContainer[i++] = segmentOrigin;
        }

        return pointsContainer;
    }

    private void FireReflectingShot2D(ref ReflectingRayInfo reflectingRay, Vector3 segmentOrigin, float range, LayerMask layersToContact, LayerMask bouncableLayers)
    {
        var direction = reflectingRay.direction;
        RaycastHit2D hit = Physics2D.Raycast(segmentOrigin, direction, range, layersToContact);
        // Ray did not hit anything.
        if (!hit)
        {            
            var maxRangePoint = GetMaxRangePoint(segmentOrigin, direction, range);
            reflectingRay.SetPositionAndDirection(maxRangePoint, direction);
        }
        else
        {
            var hitInfo = hit.collider.gameObject;
            EvaluateCollision(ref reflectingRay, bouncableLayers, hitInfo);
            reflectingRay.SetPositionAndDirection(hit.point, Vector2.Reflect(direction, hit.normal));
        }
    }

    private void FireReflectingShot3D(ref ReflectingRayInfo reflectingRay, Vector3 segmentOrigin, float range, LayerMask layersToContact, LayerMask bouncableLayers)
    {
        var direction = reflectingRay.direction;
        // Fire a ray, If it misses go into this if statement; Otherwise place the results into hit.
        if (!Physics.Raycast(segmentOrigin, direction, out RaycastHit hit, range, layersToContact))
        {
            // Create a point at the max range.
            var maxRangePoint = GetMaxRangePoint(segmentOrigin, direction, range);
            reflectingRay.SetPositionAndDirection(maxRangePoint, direction);
        }
        else
        {
            var hitInfo = hit.collider.gameObject;
            EvaluateCollision(ref reflectingRay, bouncableLayers, hitInfo);
            reflectingRay.SetPositionAndDirection(hit.point, Vector3.Reflect(direction, hit.normal));
        }
    }

    private void EvaluateCollision(ref ReflectingRayInfo reflectingRay, LayerMask bouncableLayers, GameObject hitInfo)
    {
        // Hit a non bouncable object.
        if (!IsLayerBouncable(hitInfo.layer, bouncableLayers))
        {
            reflectingRay.hitNonBouncable = true;
        }

        var alertTarget = reflectingRay.alertTarget;
        var hurtTarget = reflectingRay.hurtTarget;
        if (alertTarget || hurtTarget)
        {
            if (hitInfo.TryGetComponent(out ILaserTarget target))
            {
                target.TouchedByLaser(alertTarget, hurtTarget);
            }
        }
    }

    private Vector3 GetMaxRangePoint(Vector3 origin, Vector3 direction, float range)
    {
        return origin + (direction * range);
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
    public Vector3 segmentOrigin;
    public Vector3 direction;
    public bool hitNonBouncable;
    public bool alertTarget;
    public bool hurtTarget;

    public ReflectingRayInfo(Vector3 segmentOrigin, Vector3 direction, bool alertTarget, bool hurtTarget)
    {
        this.segmentOrigin = segmentOrigin;
        this.direction = direction;
        hitNonBouncable = false;
        this.alertTarget = alertTarget;
        this.hurtTarget = hurtTarget;
    }

    public void SetPositionAndDirection(Vector3 segmentOrigin, Vector3 direction)
    {
        this.segmentOrigin = segmentOrigin;
        this.direction = direction;
    }
}
