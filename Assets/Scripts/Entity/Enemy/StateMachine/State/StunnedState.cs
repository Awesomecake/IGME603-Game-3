using UnityEngine;

public class StunnedState : State
{
    [SerializeField] private Enemy character;

    [SerializeField] private GameObject stunnedIndicatorPrefab;
    [SerializeField] private Vector3 offset = Vector3.up;
    [SerializeField] private Collider2D visionArea;
    private GameObject _stunnedIndicator;

    private void Start()
    {
        _stunnedIndicator = Instantiate(stunnedIndicatorPrefab, character.transform);
        EnableIndicator(false);
    }

    public override void EnterState()
    {
        character.SetState(Enemy.State.Stunned);
        EnableIndicator(true);
        if (!visionArea) visionArea.enabled = false;
    }

    public override void ExitState()
    {
        if (visionArea) visionArea.enabled = true;
        character.SetState(Enemy.State.Normal);
        EnableIndicator(false);
    }

    private void EnableIndicator(bool isEnabled)
    {
        _stunnedIndicator.transform.position = character.transform.position + offset;
        _stunnedIndicator?.SetActive(isEnabled);
    }
}