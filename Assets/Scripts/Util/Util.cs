using System;
using System.Collections;
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
}