using UnityEngine;

public class DownedStateMachine : HierarchicalStateMachine
{
    [SerializeField] private LilBro self;
    [SerializeField] private Hurtbox hurtbox;

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

        hurtbox.gameObject.SetActive(false);
        if (!_broCollider) return;
        _broCollider.enabled = false;
    }

    public override void ExitState()
    {
        self.RandomizeHeldItem();
        self.UpdateState(LilBro.State.Normal);
        if (_broCollider) _broCollider.enabled = true;
        hurtbox.gameObject.SetActive(true);
        base.ExitState();
    }
}