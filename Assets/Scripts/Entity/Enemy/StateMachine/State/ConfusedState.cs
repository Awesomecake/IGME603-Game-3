using UnityEngine;

public class ConfusedState : State
{
    [SerializeField] private Enemy character;
    [SerializeField] private GameObject confusedIndicatorPrefab;

    [SerializeField] private Vector3 offset = Vector3.up;
    [SerializeField] private float rotationRange = 15f;
    [SerializeField] private float rotationPeriod = 2f;
    private GameObject _confusedIndicator;

    private float _enterTime;
    private float _baseRotation;


    private void Start()
    {
        _confusedIndicator = Instantiate(confusedIndicatorPrefab, character.transform);
        EnableIndicator(false);
    }
    public override void EnterState()
    {
        character.body.velocity = Vector3.zero;

        _baseRotation = character.transform.rotation.eulerAngles.z;
        _enterTime = Time.time;
        EnableIndicator(true);
    }

    public override void ExitState()
    {
        character.SetState(Enemy.State.Normal);
        EnableIndicator(false);
    }

    public override void StateUpdate()
    {
        var timeSpent = Time.time - _enterTime;
        const float twoPi = Mathf.PI * 2f;
        var currentOffset = rotationRange * Mathf.Sin(twoPi * timeSpent / rotationPeriod);
        character.transform.rotation = Quaternion.Euler(0f, 0f, _baseRotation + currentOffset);
    }

    private void EnableIndicator(bool isEnabled)
    {
        _confusedIndicator.transform.position = character.transform.position + offset;
        _confusedIndicator?.SetActive(isEnabled);
    }
}