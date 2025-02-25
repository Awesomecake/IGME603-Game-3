using UnityEngine;

public class TriggeredStateTransition : StateTransition
{
    [SerializeField] private EventTrigger trigger;

    private HierarchicalStateMachine _stateMachine;
    private bool _needsTransition = false;
    private Coroutine _resetCoroutine;

    private void TriggerTransition()
    {
        if (_stateMachine.GetCurrentState() != fromState) return;
        _needsTransition = true;
    }

    private void Start()
    {
        _stateMachine = GetComponent<HierarchicalStateMachine>();
        trigger.onTrigger.AddListener(TriggerTransition);
        _resetCoroutine = StartCoroutine(Util.AfterDelay(
            delaySeconds: 0.5f,
            lambda: () => _needsTransition = false
        ));
    }

    private void OnDestroy()
    {
        trigger?.onTrigger?.RemoveListener(TriggerTransition);
    }

    public override bool NeedsTransition()
    {
        if (_resetCoroutine != null) StopCoroutine(_resetCoroutine);
        if (!_needsTransition) return false;

        _needsTransition = false;
        return true;
    }
}