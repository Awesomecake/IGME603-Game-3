using UnityEngine;

public class CombatStateMachine : HierarchicalStateMachine
{
    [SerializeField] private SpriteRenderer visionCone;
    private Color _originalVisionColor;
    [SerializeField] private Color spottedVisionColor = Color.red.Copy(a: 0.2f);
    [SerializeField] private Enemy self;
    
    protected override void Start()
    {
        _originalVisionColor = visionCone.color;
        base.Start();
    }

    public override void EnterState()
    {
        self.SetState(Enemy.State.Chasing);
        SetVisionColor(spottedVisionColor);
        base.EnterState();
    }

    public override void ExitState()
    {
        self.SetState(Enemy.State.Normal);
        SetVisionColor(_originalVisionColor);
        base.ExitState();
    }
    
    private void SetVisionColor(Color color)
    {
        visionCone.color = color;
    }
}