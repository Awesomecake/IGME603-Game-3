using UnityEngine;

public class ConfusedState : State
{
    [SerializeField] private Rigidbody2D character;

    [SerializeField] private float rotationRange = 15f;
    [SerializeField] private float rotationPeriod = 2f;

    private float _enterTime;
    private float _baseRotation;

    public override void EnterState()
    {
        character.velocity = Vector3.zero;

        _baseRotation = character.transform.rotation.eulerAngles.z;
        _enterTime = Time.time;
    }

    public override void StateUpdate()
    {
        var timeSpent = Time.time - _enterTime;
        const float twoPi = Mathf.PI * 2f;
        var currentOffset = rotationRange * Mathf.Sin(twoPi * timeSpent / rotationPeriod);
        character.transform.rotation = Quaternion.Euler(0f, 0f, _baseRotation + currentOffset);
    }
}