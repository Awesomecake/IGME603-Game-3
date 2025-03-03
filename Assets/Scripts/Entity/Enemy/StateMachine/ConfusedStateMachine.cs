using UnityEngine;

public class ConfusedStateMachine : HierarchicalStateMachine
{
    [SerializeField] private Enemy self;
    public override void EnterState()
    {
        base.EnterState();
        self.SetState(Enemy.State.Confused);
    }

    public override void ExitState()
    {
        self.SetState(Enemy.State.Normal);
        base.ExitState();
    }
}