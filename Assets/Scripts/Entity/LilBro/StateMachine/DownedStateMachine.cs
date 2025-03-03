using UnityEngine;

public class DownedStateMachine : HierarchicalStateMachine
{
    [SerializeField] private LilBro self;
    public override void EnterState()
    {
        base.EnterState();
        self.currentState = LilBro.State.Downed;
    }

    public override void ExitState()
    {
        self.currentState = LilBro.State.Normal;
        base.ExitState();
    }
}