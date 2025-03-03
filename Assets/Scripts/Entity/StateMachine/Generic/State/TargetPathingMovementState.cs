using UnityEngine;
using UnityEngine.Events;

public class TargetPathingMovementState : State
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] protected PathContainer pathContainer;
    [SerializeField] private float distanceThreshold = 0.2f;
    [SerializeField] private AbstractTargetContainer simpleTargetContainer;

    public override void EnterState()
    {
        base.EnterState();
        UpdatePath();
    }

    private void UpdatePath()
    {
        var path = WorldManager.Instance.FindPath(
            transform.position,
            simpleTargetContainer.GetLocation()
        );
        pathContainer.SetPath(path);
    }

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
            UpdatePath();
        }
    }

    public override void ExitState()
    {
        body.velocity = Vector2.zero;
    }
}