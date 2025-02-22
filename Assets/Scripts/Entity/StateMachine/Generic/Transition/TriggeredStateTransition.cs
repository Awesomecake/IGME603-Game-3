public class TriggeredStateTransition : StateTransition
{
    private HierarchicalStateMachine _stateMachine;
    private bool _needsTransition = false;

    public void TriggerTransition()
    {
        if (_stateMachine.GetCurrentState() != fromState) return;
    }

    private void Start()
    {
        _stateMachine = GetComponent<HierarchicalStateMachine>();
    }

    public override bool NeedsTransition()
    {
        if (!_needsTransition) return false;
        
        _needsTransition = false;
        return true;

    }
}