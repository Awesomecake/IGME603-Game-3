using UnityEngine;

public class SeekGoalState : PathMovementState
{
    public override void EnterState()
    {
        base.EnterState();
        var world = WorldManager.Instance;
        var path = world.FindPath(
            transform.position,
            world.diamond.transform.position
        );
        pathContainer.SetPath(path);
    }
}