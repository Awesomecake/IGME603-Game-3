using UnityEngine;
using UnityEngine.Events;

public class SimpleTargetMovementState : State
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private TargetContainer simpleTargetContainer;
    
    public override void PhysicsUpdate()
    {
        var target = simpleTargetContainer.GetCurrentTarget();
        if (!target) return;

        var trueSpeed = movementSpeed * Time.fixedTime;
        var vectorToTarget = target.position - body.transform.position;
        var directionToTarget = vectorToTarget.normalized;
        var facingAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        
        body.rotation = facingAngle;
        body.velocity = trueSpeed * directionToTarget;
    }

    public override void Exit()
    {
        body.velocity = Vector2.zero;
    }
}