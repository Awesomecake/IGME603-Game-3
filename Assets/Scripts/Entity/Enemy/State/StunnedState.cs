using UnityEngine;

public class StunnedState: State
{
    [SerializeField] private Transform character;

    [SerializeField] private GameObject stunnedIndicatorPrefab;
    [SerializeField] private Vector3 offset = Vector3.up;
    private GameObject _stunnedIndicator;

    private void Start()
    {
        _stunnedIndicator = Instantiate(stunnedIndicatorPrefab, character);
        EnableIndicator(false);
    }
    
    public override void EnterState()
    {
        EnableIndicator(true);
    }

    public override void ExitState()
    {
        EnableIndicator(false);
    }
    
    private void EnableIndicator(bool isEnabled)
    {
        _stunnedIndicator.transform.position = character.position + offset;
        _stunnedIndicator?.SetActive(isEnabled);
    }
}