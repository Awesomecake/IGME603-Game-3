using System.Collections;
using UnityEngine;

public class DelayedTriggerStateTransition : TriggeredStateTransition
{
    public enum DuplicateInvocationMode
    {
        ContinuePrevious,
        ResetTimer
    }

    [SerializeField] private DuplicateInvocationMode mode = DuplicateInvocationMode.ResetTimer;

    [SerializeField] private float delaySeconds;
    private Coroutine _delay;

    protected override void Transition()
    {
        switch (mode)
        {
            case DuplicateInvocationMode.ContinuePrevious:
                if (_delay != null) return;
                break;
            
            case DuplicateInvocationMode.ResetTimer:
            default:
                CancelTransition();
                break;
        }

        _delay = StartCoroutine(Delay());
    }

    public void CancelTransition()
    {
        if (_delay != null) StopCoroutine(_delay);
        _delay = null;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(delaySeconds);
        base.Transition();
        yield return null;
    }

    public override bool NeedsTransition()
    {
        if (StateMachine.GetCurrentState() != fromState)
        {
            CancelTransition();
        }

        return false;
    }
}