public class SeekGoalState : PathMovementState
{
    public override void EnterState()
    {
        base.EnterState();
        var world = WorldManager.Instance;
        var path = world.FindPath(
            transform.position,
            world.diamond.transform.position,
            10000
        );
        pathContainer.SetPath(path);
    }
}