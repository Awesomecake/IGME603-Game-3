using UnityEngine;

public class AttackState : State
{
    [SerializeField] private Enemy self;
    [SerializeField] private GameObject alertIndicatorPrefab;
    [SerializeField] private Vector3 offset = Vector3.up;
    [SerializeField] private AbstractTargetContainer targetContainer;
    [SerializeField] private Throwable bulletPrefab;
    [SerializeField] private float velocity = 500f;
    private GameObject _alertIndicator;

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
        
        bullet.ThrowItem(velocity, direction, self.gameObject);
    }
}