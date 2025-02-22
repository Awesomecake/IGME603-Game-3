using UnityEngine;

public class TargetContainer : MonoBehaviour
{
    [SerializeField] private Transform target;

    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public virtual Transform GetCurrentTarget()
    {
        return target;
    }
}