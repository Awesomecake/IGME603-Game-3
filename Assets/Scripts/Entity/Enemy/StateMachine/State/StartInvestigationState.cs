using UnityEngine;

public class StartInvestigationState : State
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private PathContainer pathContainer;
    [SerializeField] private AbstractTargetContainer locationContainer;

    public override void EnterState()
    {
        base.EnterState();

        var startPosition = body.transform.position;
        var endPosition = locationContainer.GetLocation();

        var path = WorldManager.Instance.FindPath(
            startPosition,
            endPosition
        );
        if (path.Count <= 0)
        {
            path = Util.GetPathToNearestWall(startPosition, endPosition);
        }

        pathContainer.SetPath(path);
    }
}