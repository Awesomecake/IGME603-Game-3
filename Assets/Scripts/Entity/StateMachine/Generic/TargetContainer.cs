using UnityEngine;

public class TargetContainer : AbstractTargetContainer
{
    [SerializeField] private Transform target;
    private Vector3 _lastKnownPosition = new Vector3();

    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public override Vector3 GetCurrentTarget()
    {
        if (target)
        {
            _lastKnownPosition = target.position;
        }
        return _lastKnownPosition;
    }
}