using UnityEngine;

public class PatrolHandler : MonoBehaviour
{
    [SerializeField] private TargetContainer patrolTarget;
    [SerializeField] private Path path;

    private Path.Iterator _iterator;

    private void Start()
    {
        _iterator = path.GetNearest(transform.position);
        UpdateTarget();
    }

    public void SetPath(Path newPath)
    {
        path = newPath;
    }

    public void NextNode()
    {
        _iterator = path.GetNextIterator(_iterator);
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        patrolTarget.UpdateTarget(path.GetNode(_iterator));
    }
}