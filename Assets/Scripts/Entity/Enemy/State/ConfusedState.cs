using UnityEngine;

public class ConfusedState : State
{
    [SerializeField] private Transform character;
    
    [SerializeField] private float rotationRange = 15f;
    [SerializeField] private float rotationPeriod = 2f;

    private float _enterTime;
    private float _baseRotation;

    public override void EnterState()
    {
        _baseRotation = character.rotation.eulerAngles.z;
        _enterTime = Time.time;
    }

    public override void StateUpdate()
    {
        var timeSpent = Time.time - _enterTime;
        const float twoPi = Mathf.PI * 2f;
        var currentOffset = rotationRange * Mathf.Sin(twoPi * timeSpent / rotationPeriod);
        character.rotation = Quaternion.Euler(0f, 0f, _baseRotation + currentOffset);
    }
}