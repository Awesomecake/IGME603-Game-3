using UnityEngine;

public class ConfusedState : State
{
    [SerializeField] private Transform character;
    
    [SerializeField] private GameObject confusedIndicatorPrefab;
    [SerializeField] private Vector3 offset = Vector3.up;
    private GameObject _confusedIndicator;

    [SerializeField] private float rotationRange = 15f;
    [SerializeField] private float rotationPeriod = 2f;

    private float _enterTime;
    private float _baseRotation;
    
    private void Start()
    {
        _confusedIndicator = Instantiate(confusedIndicatorPrefab);
        EnableIndicator(false);
    }

    public override void Enter()
    {
        _baseRotation = character.rotation.eulerAngles.z;
        _enterTime = Time.time;
        EnableIndicator(true);
    }

    public override void Exit()
    {
        EnableIndicator(false);
    }

    public override void FrameUpdate()
    {
        var timeSpent = Time.time - _enterTime;
        const float twoPi = Mathf.PI * 2f;
        var currentOffset = rotationRange * Mathf.Sin(twoPi * timeSpent / rotationPeriod);
        character.rotation = Quaternion.Euler(0f, 0f, _baseRotation + currentOffset);
    }

    private void EnableIndicator(bool isEnabled)
    {
        _confusedIndicator.transform.position = character.position + offset;
        _confusedIndicator?.SetActive(isEnabled);
    }
}