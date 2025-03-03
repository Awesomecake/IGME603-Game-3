using UnityEngine;

public class DownedStateMachine : HierarchicalStateMachine
{
    [SerializeField] private LilBro self;

    public override void EnterState()
    {
        base.EnterState();
        self.UpdateState(LilBro.State.Downed);
        var broCollider = self.GetComponent<Collider2D>();

        if (!broCollider) return;
        broCollider.enabled = false;
        StartCoroutine(Util.AfterDelay(
            0.1f,
            () => broCollider.enabled = true
        ));
    }

    public override void ExitState()
    {
        self.UpdateState(LilBro.State.Normal);
        base.ExitState();
    }
}