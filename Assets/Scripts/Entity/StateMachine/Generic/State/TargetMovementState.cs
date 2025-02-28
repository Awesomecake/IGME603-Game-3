using UnityEngine;

public class TargetMovementState : State
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private AbstractTargetContainer simpleTargetContainer;
    
    public override void StateFixedUpdate()
    {
        var target = simpleTargetContainer.GetLocation();
        var directionToTarget = (target - body.transform.position).normalized;
        var facingAngle = body.transform.position.GetAngleTowards2D(target);
        
        body.rotation = facingAngle;
        body.velocity = movementSpeed * directionToTarget;
    }

    public override void ExitState()
    {
        body.velocity = Vector2.zero;
    }
}