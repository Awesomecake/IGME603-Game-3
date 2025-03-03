using UnityEngine;

public class SeekSpawnState : PathMovementState
{
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    public override void EnterState()
    {
        base.EnterState();
        var path = WorldManager.Instance.FindPath(
            transform.position,
            _startPosition
        );
        pathContainer.SetPath(path);
    }
}