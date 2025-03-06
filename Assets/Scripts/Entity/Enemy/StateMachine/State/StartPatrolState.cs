using UnityEngine;

public class StartPatrolState : State
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private PathContainer pathContainer;
    [SerializeField] private AbstractTargetContainer locationContainer;
    [SerializeField] private PatrolAreaHandler patrolAreaHandler;

    public override void EnterState()
    {
        base.EnterState();

        patrolAreaHandler.NextPoint();

        var path = WorldManager.Instance.FindPath(
            body.transform.position,
            locationContainer.GetLocation()
        );
        if (path.Count <= 0)
        {
            path = Util.GetPathToNearestWall(body.transform.position, locationContainer.GetLocation());
        }

        pathContainer.SetPath(path);
    }
}