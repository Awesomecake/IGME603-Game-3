using UnityEngine;

public class LilBro : MonoBehaviour
{
    public enum State
    {
        Normal,
        Downed,
    }

    [SerializeField] private SpriteRenderer spriteRenderer;

    private State _currentState = State.Normal;
    private Color _defaultColor;

    private void Start()
    {
        if (!spriteRenderer) GetComponent<SpriteRenderer>();
        if (spriteRenderer) _defaultColor = spriteRenderer.color;
    }

    public void UpdateState(State newState)
    {
        _currentState = newState;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (!spriteRenderer) return;
        switch (_currentState)
        {
            case State.Downed:
                spriteRenderer.color = Color.blue;
                break;

            case State.Normal:
            default:
                spriteRenderer.color = _defaultColor;
                break;
        }
    }

    public State GetCurrentState()
    {
        return _currentState;
    }
}