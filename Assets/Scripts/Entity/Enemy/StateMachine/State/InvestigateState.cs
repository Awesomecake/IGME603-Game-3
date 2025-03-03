using UnityEngine;

public class InvestigateState : PathMovementState
{
    [SerializeField] private AbstractTargetContainer locationContainer;

    public override void EnterState()
    {
        base.EnterState();
        var path = WorldManager.Instance.FindPath(
            body.transform.position,
            locationContainer.GetLocation()
        );
        pathContainer.SetPath(path);
    }
}