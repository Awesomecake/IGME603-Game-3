using UnityEngine;

public class AlertedState : State
{
    [SerializeField] private AbstractTargetContainer target;
    [SerializeField] private Rigidbody2D body;

    public override void StateFixedUpdate()
    {
        var location = target.GetLocation();
        var facingAngle = body.transform.position.GetAngleTowards2D(location);
        body.rotation = facingAngle;
    }
}