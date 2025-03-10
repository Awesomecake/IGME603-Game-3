using UnityEngine;

public class StunnedState : State
{
    [SerializeField] private Enemy character;

    [SerializeField] private GameObject stunnedIndicatorPrefab;
    [SerializeField] private Vector3 offset = Vector3.up;
    [SerializeField] [Range(0f, 1f)] private float friction = 0.05f;
    private GameObject _stunnedIndicator;

    private void Start()
    {
        //_stunnedIndicator = Instantiate(stunnedIndicatorPrefab, character.transform);
        //EnableIndicator(false);
    }

    public override void EnterState()
    {
        character.SetState(Enemy.State.Stunned);
        //EnableIndicator(true);
    }

    public override void ExitState()
    {
        character.SetState(Enemy.State.Normal);
       // EnableIndicator(false);
    }

    public override void StateFixedUpdate()
    {
        character.body.velocity *= (1f - friction);
    }

    private void EnableIndicator(bool isEnabled)
    {
        _stunnedIndicator.transform.position = character.transform.position + offset;
        _stunnedIndicator?.SetActive(isEnabled);
    }
}