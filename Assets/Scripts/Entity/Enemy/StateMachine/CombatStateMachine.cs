using UnityEngine;

public class CombatStateMachine : HierarchicalStateMachine
{
    private static readonly int ColorId = Shader.PropertyToID("_Color");

    // [SerializeField] private SpriteRenderer visionCone;
    [SerializeField] private MeshRenderer visionCone;
    private Color _originalVisionColor;
    [SerializeField] private Color spottedVisionColor = Color.red.Copy(a: 0.2f);
    [SerializeField] private Enemy self;
    
    protected override void Start()
    {
        _originalVisionColor = visionCone.material.color;
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
        visionCone.material.SetColor(ColorId, color);
    }
}