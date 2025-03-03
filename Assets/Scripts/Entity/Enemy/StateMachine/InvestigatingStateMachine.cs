using UnityEngine;

public class InvestigatingStateMachine : HierarchicalStateMachine
{
    [SerializeField] private Enemy self;
    public override void EnterState()
    {
        base.EnterState();
        self.SetState(Enemy.State.Investigating);
    }

    public override void ExitState()
    {
        self.SetState(Enemy.State.Normal);
        base.ExitState();
    }
}