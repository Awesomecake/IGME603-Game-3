using UnityEngine;

public class TriggeredStateTransition : StateTransition
{
    [SerializeField] private EventTrigger trigger;

    protected HierarchicalStateMachine StateMachine;

    private void Start()
    {
        StateMachine = GetComponent<HierarchicalStateMachine>();
        trigger.onTrigger.AddListener(OnTriggerActivated);
    }

    private void OnDestroy()
    {
        trigger?.onTrigger?.RemoveListener(OnTriggerActivated);
    }

    private void OnTriggerActivated()
    {
        Transition();
    }

    protected virtual void Transition()
    {
        if (StateMachine.GetCurrentState() != fromState) return;
        StateMachine.ChangeState(toState);
    }

    public override bool NeedsTransition()
    {
        return false;
    }
}