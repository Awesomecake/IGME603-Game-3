using System.Collections;
using UnityEngine;

public class DelayedTriggerStateTransition : TriggeredStateTransition
{
    [SerializeField] private float delaySeconds;
    private Coroutine _delay;

    protected override void Transition()
    {
        if (_delay != null) CancelTransition();
        _delay = StartCoroutine(Delay());
    }

    public void CancelTransition()
    {
        StopCoroutine(_delay);
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