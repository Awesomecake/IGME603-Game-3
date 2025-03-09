using System.Linq;
using UnityEngine;

public class SeekSpawnState : PathMovementState
{
    private Vector3 _startPosition;

    public int stepLimit = 10;

    private void Start()
    {
        _startPosition = transform.position;
    }

    public override void EnterState()
    {
        base.EnterState();
        var path = WorldManager.Instance.FindPath(
            transform.position,
            _startPosition,
            10000
        ).Take(stepLimit).ToList();
        pathContainer.SetPath(path);
    }
}