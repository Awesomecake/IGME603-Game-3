using UnityEngine;

public class DistanceStateTransition : StateTransition
{
    [SerializeField] private TargetContainer targetContainer;
    [SerializeField] private Transform body;
    [SerializeField] private float distanceThreshold = 1f;
    
    public override bool NeedsTransition()
    {
        var target = targetContainer.GetCurrentTarget();
        if (!target) return false;

        var squaredDistanceThreshold = distanceThreshold * distanceThreshold;
        
        var currentSquaredDistance = body.position.DistanceTo2DSquared(target.position);

        return currentSquaredDistance <= squaredDistanceThreshold;
    }
}