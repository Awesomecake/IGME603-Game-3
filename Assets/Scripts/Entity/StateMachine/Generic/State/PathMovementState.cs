using UnityEngine;
using UnityEngine.Events;

public class PathMovementState : State
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] protected PathContainer pathContainer;
    [SerializeField] private float distanceThreshold = 0.2f;

    public UnityEvent onPathEnded;
    
    public override void StateFixedUpdate()
    {
        var threshold = distanceThreshold * distanceThreshold;
        var target = pathContainer.GetLocation();
        var directionToTarget = (target - body.transform.position).normalized;
        var facingAngle = body.transform.position.GetAngleTowards2D(target);
        
        body.rotation = facingAngle;
        body.velocity = movementSpeed * directionToTarget;

        var isAtTarget = body.transform.position.DistanceTo2DSquared(target) <= threshold;
        if (isAtTarget)
        {
            var isFinished = pathContainer.NextPoint();
            if (isFinished) onPathEnded?.Invoke();
        }
    }

    public override void ExitState()
    {
        body.velocity = Vector2.zero;
    }
}