using UnityEngine;

public class TriggeredStateTransition : StateTransition
{
    [SerializeField] private EventTrigger trigger;

    private HierarchicalStateMachine _stateMachine;

    private void Start()
    {
        _stateMachine = GetComponent<HierarchicalStateMachine>();
        trigger.onTrigger.AddListener(TriggerTransition);
    }

    private void OnDestroy()
    {
        trigger?.onTrigger?.RemoveListener(TriggerTransition);
    }

    private void TriggerTransition()
    {
        if (_stateMachine.GetCurrentState() != fromState) return;
        _stateMachine.ChangeState(toState);
    }

    public override bool NeedsTransition()
    {
        return false;
    }
}