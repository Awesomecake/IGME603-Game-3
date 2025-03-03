using UnityEngine;

public class NormalStateMachine : HierarchicalStateMachine
{
    [SerializeField] private Enemy self;
    public override void EnterState()
    {
        base.EnterState();
        self.SetState(Enemy.State.Normal);
    }

    public override void ExitState()
    {
        self.SetState(Enemy.State.Normal);
        base.ExitState();
    }
}