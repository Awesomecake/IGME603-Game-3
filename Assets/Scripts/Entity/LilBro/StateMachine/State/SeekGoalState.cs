using UnityEngine;

public class SeekGoalState : PathMovementState
{
    public override void EnterState()
    {
        Debug.Log("Entered Path Seek State");
        base.EnterState();
        var world = WorldManager.Instance;
        var path = world.FindPath(
            transform.position,
            world.diamond.transform.position
        );
        Debug.Log($"Found path {path}");
        pathContainer.SetPath(path);
    }
}