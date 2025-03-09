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

    [SerializeField] private ItemPool itemPool;
    private GameObject _heldItemPrefab;
    [SerializeField] private SpriteRenderer heldItemRenderer;

    private void Start()
    {
        if (!spriteRenderer) GetComponent<SpriteRenderer>();
        if (spriteRenderer) _defaultColor = spriteRenderer.color;
        
        RandomizeHeldItem();
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

    public void RandomizeHeldItem()
    {
        SetHeldItemPrefab(itemPool.RandomItem());
    }

    public void ClearHeldItem()
    {
        SetHeldItemPrefab(null);
    }

    public GameObject GetHeldItemPrefab()
    {
        return _heldItemPrefab;
    }

    private void SetHeldItemPrefab(GameObject prefab)
    {
        if (!prefab)
        {
            heldItemRenderer.sprite = null;
            _heldItemPrefab = null;
            return;
        }
        var image = prefab.GetComponent<SpriteRenderer>().sprite;
        heldItemRenderer.sprite = image;
        _heldItemPrefab = prefab;
    }
}