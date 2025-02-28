using UnityEngine;

public class CombatStateMachine : HierarchicalStateMachine
{
    [SerializeField] private SpriteRenderer visionCone;
    private Color _originalVisionColor;
    [SerializeField] private Color spottedVisionColor = Color.red.Copy(a: 0.2f);
    
    protected override void Start()
    {
        _originalVisionColor = visionCone.color;
        base.Start();
    }

    public override void EnterState()
    {
        SetVisionColor(spottedVisionColor);
        base.EnterState();
    }

    public override void ExitState()
    {
        SetVisionColor(_originalVisionColor);
        base.ExitState();
    }
    
    private void SetVisionColor(Color color)
    {
        visionCone.color = color;
    }
}