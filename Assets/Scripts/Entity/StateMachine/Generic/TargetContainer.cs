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
        if (target && target.gameObject.activeSelf)
        {
            _lastKnownPosition = target.position.Copy();
        }
        return _lastKnownPosition;
    }
}