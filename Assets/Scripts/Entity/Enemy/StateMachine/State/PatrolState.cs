using System.Collections.Generic;
using UnityEngine;

public class PatrolState : PathMovementState
{
    [SerializeField] private AbstractTargetContainer locationContainer;
    [SerializeField] private PatrolAreaHandler patrolAreaHandler;

    public override void EnterState()
    {
        base.EnterState();
        var path = new List<Vector3>();
        while (path.Count <= 0)
        {
            patrolAreaHandler.NextPoint();
            path = WorldManager.Instance.FindPath(
                body.transform.position,
                locationContainer.GetLocation()
            );
        }

        pathContainer.SetPath(path);
    }
}