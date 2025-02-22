using UnityEngine;

public class CooldownTransition : StateTransition
{
    [SerializeField] private float durationSeconds;
    
    private HierarchicalStateMachine _stateMachine;

    private void Start()
    {
        _stateMachine = GetComponent<HierarchicalStateMachine>();
    }

    public override bool NeedsTransition()
    {
        var timeElapsed = Time.time - _stateMachine.lastTransition;
        return timeElapsed >= durationSeconds;
    }
}