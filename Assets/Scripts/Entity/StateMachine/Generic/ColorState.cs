using UnityEngine;

public class ColorState: State
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color color = Color.red;

    private Color _initialColor;
    
    public override void Enter()
    {
        _initialColor = spriteRenderer.color;
        spriteRenderer.color = color;
    }

    public override void Exit()
    {
        spriteRenderer.color = _initialColor;
    }
}