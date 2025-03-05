using UnityEngine;

public class TargetContainer : AbstractTargetContainer
{
    [SerializeField] private Transform target;
    private Vector3 _lastKnownPosition = new Vector3();

    public void UpdateTarget(Transform newTarget)
    {
        if (!newTarget) return;
        target = newTarget;
        _lastKnownPosition = target.position.Copy();
    }

    public override Vector3 GetLocation()
    {
        if (target && target.gameObject.activeSelf)
        {
            _lastKnownPosition = target.position.Copy();
        }
        return _lastKnownPosition;
    }
}