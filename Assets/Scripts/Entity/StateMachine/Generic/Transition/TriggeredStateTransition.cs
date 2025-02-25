using UnityEngine;

public class TriggeredStateTransition : StateTransition
{
    [SerializeField] private EventTrigger trigger;
    
    private HierarchicalStateMachine _stateMachine;
    private bool _needsTransition = false;

    private void TriggerTransition()
    {
        if (_stateMachine.GetCurrentState() != fromState) return;
        _needsTransition = true;
    }

    private void Start()
    {
        _stateMachine = GetComponent<HierarchicalStateMachine>();
        trigger.onTrigger.AddListener(TriggerTransition);
        StartCoroutine(Util.AfterDelay(0.5f, () => _needsTransition = false));
    }

    private void OnDestroy()
    {
        trigger?.onTrigger?.RemoveListener(TriggerTransition);
    }

    public override bool NeedsTransition()
    {
        StopAllCoroutines();
        if (!_needsTransition) return false;
        
        _needsTransition = false;
        return true;

    }
}