using UnityEngine;

public class LocationContainer : AbstractTargetContainer
{
    private Vector3 _lastKnownPosition = new Vector3();

    public void UpdatePosition(Vector3 newPosition)
    {
        _lastKnownPosition = newPosition;
    }

    public override Vector3 GetLocation()
    {
        return _lastKnownPosition;
    }
}