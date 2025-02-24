using UnityEngine;

public class ConfusedState : State
{
    [SerializeField] private GameObject confusedIndicatorPrefab;
    [SerializeField] private Vector3 offset = Vector3.up;
    private GameObject _confusedIndicator;

    private void Start()
    {
        _confusedIndicator = Instantiate(confusedIndicatorPrefab);
        EnableIndicator(false);
    }

    public override void Enter()
    {
        EnableIndicator(true);
    }

    public override void Exit()
    {
        EnableIndicator(false);
    }

    private void EnableIndicator(bool isEnabled)
    {
        _confusedIndicator.transform.position = transform.position + offset;
        _confusedIndicator?.SetActive(isEnabled);
    }
}