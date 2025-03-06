using UnityEngine;
using UnityEngine.Events;

public class RotationState : State
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float turningSpeed = 30f;
    [SerializeField] private AbstractTargetContainer targetContainer;

    [SerializeField] private UnityEvent onFacingTarget;
    
    public override void StateFixedUpdate()
    {
        var targetRotation = body.transform.position.GetAngleTowards2D(targetContainer.GetLocation());
        var angleChange = turningSpeed * Time.fixedDeltaTime;
        var remaining = Mathf.DeltaAngle(body.rotation, targetRotation);
        body.rotation = Mathf.MoveTowardsAngle(body.rotation, targetRotation, angleChange);

        if (Mathf.Abs(remaining) < angleChange)
        {
            onFacingTarget?.Invoke();
        }
    }
}