using UnityEngine;

public class TargetMovementState : State
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private TargetContainer simpleTargetContainer;
    
    public override void PhysicsUpdate()
    {
        var target = simpleTargetContainer.GetCurrentTarget();
        if (!target) return;

        var vectorToTarget = target.position - body.transform.position;
        var directionToTarget = vectorToTarget.normalized;
        var facingAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        
        body.rotation = facingAngle;
        body.velocity = movementSpeed * directionToTarget;
    }

    public override void Exit()
    {
        body.velocity = Vector2.zero;
    }
}