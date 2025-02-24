using System;
using UnityEngine;

public class ChaseState : EnemyMovementState
{
    [SerializeField] private SpriteRenderer visionCone;
    private Color _originalVisionColor;
    [SerializeField] private Color spottedVisionColor = Color.red.Copy(a: 0.2f);

    private void Start()
    {
        _originalVisionColor = visionCone.color;
    }

    public override void EnterState()
    {
        SetVisionColor(spottedVisionColor);
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