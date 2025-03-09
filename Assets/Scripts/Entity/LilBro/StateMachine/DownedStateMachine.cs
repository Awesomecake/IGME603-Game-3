using UnityEngine;

public class DownedStateMachine : HierarchicalStateMachine
{
    [SerializeField] private LilBro self;

    private Collider2D _broCollider;

    protected override void Start()
    {
        base.Start();
        _broCollider = self.GetComponent<Collider2D>();
    }

    public override void EnterState()
    {
        base.EnterState();
        self.UpdateState(LilBro.State.Downed);

        if (!_broCollider) return;
        _broCollider.enabled = false;
    }

    public override void ExitState()
    {
        self.RandomizeHeldItem();
        self.UpdateState(LilBro.State.Normal);
        if (_broCollider) _broCollider.enabled = true;
        base.ExitState();
    }
}