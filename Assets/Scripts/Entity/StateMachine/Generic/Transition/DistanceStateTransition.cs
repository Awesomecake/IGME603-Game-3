using System;
using UnityEngine;

public class DistanceStateTransition : StateTransition
{
    [Serializable] private enum Mode
    {
        LessThan,
        GreaterThan
    }

    [SerializeField] private Mode mode = Mode.LessThan;
    [SerializeField] private AbstractTargetContainer targetContainer;
    [SerializeField] private Transform body;
    [SerializeField] private float distanceThreshold = 1f;

    public override bool NeedsTransition()
    {
        var target = targetContainer.GetLocation();
        var squaredDistanceThreshold = distanceThreshold * distanceThreshold;
        var currentSquaredDistance = body.position.DistanceTo2DSquared(target);

        switch (mode)
        {
            case Mode.GreaterThan:
                return currentSquaredDistance >= squaredDistanceThreshold;
            case Mode.LessThan:
            default:
                return currentSquaredDistance <= squaredDistanceThreshold;
        }
    }
}