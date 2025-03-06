using UnityEngine;

public class AttackState : State
{
    [SerializeField] private Enemy self;
    [SerializeField] private AbstractTargetContainer targetContainer;
    [SerializeField] private Throwable bulletPrefab;
    [SerializeField] private float velocity = 500f;

    public override void EnterState()
    {
        base.EnterState();
        var bullet = Instantiate(
            original: bulletPrefab,
            position: self.transform.position,
            rotation: Quaternion.identity
        );

        var target = targetContainer.GetLocation();
        var direction = (target - self.transform.position).ToVector2().normalized;
        
        bullet.ThrowItem(velocity, direction);
    }
}