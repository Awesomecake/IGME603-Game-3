using UnityEngine;

public class TargetContainer : AbstractTargetContainer
{
    [SerializeField] private Transform target;

    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public override Transform GetCurrentTarget()
    {
        return target;
    }
}