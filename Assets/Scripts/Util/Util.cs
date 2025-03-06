using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static Vector3 Copy(this Vector3 self, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(
            x ?? self.x,
            y ?? self.y,
            z ?? self.z
        );
    }

    public static Vector2 ToVector2(this Vector3 self)
    {
        return new Vector2(self.x, self.y);
    }
    
    public static Color Copy(this Color self, float? r = null, float? g = null, float? b = null, float? a = null)
    {
        return new Color(
            r ?? self.r,
            g ?? self.g,
            b ?? self.b,
            a ?? self.a
        );
    }

    public static float DistanceTo2DSquared(this Vector3 self, Vector3 other)
    {
        var xDiff = self.x - other.x;
        var yDiff = self.y - other.y;
        return xDiff * xDiff + yDiff * yDiff;
    }

    public static float DistanceTo2D(this Vector3 self, Vector3 other)
    {
        return Mathf.Sqrt(self.DistanceTo2DSquared(other));
    }

    public static IEnumerator AfterDelay(float delaySeconds, Action lambda)
    {
        yield return new WaitForSeconds(delaySeconds);
        lambda?.Invoke();
    }

    public static float GetAngleTowards2D(this Vector3 self, Vector3 target)
    {
        var directionToTarget = target - self;
        return Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
    }
    
    public static List<Vector3> GetPathToNearestWall(Vector3 startPosition, Vector3 endPosition, string tag = "Untagged")
    {
        var direction = (endPosition - startPosition).normalized;
        var distance = startPosition.DistanceTo2D(endPosition);
        var hits = Physics2D.RaycastAll(
            startPosition,
            direction,
            distance
        );
        var newEndPosition = endPosition;
        foreach (var hit in hits)
        {
            var isWall = hit.collider.CompareTag(tag);
            if (!isWall) continue;
                
            newEndPosition = hit.point;
            newEndPosition -= direction * 0.5f;
            break;
        }

        return new List<Vector3>
        {
            startPosition,
            newEndPosition
        };
    }
}