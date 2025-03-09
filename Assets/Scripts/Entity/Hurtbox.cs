using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : MonoBehaviour
{
    public UnityEvent onHurt;
    public Collider2D owner;

    private void Start()
    {
        if (!owner) owner = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var throwable = other.GetComponent<Throwable>();
        if (!throwable) return;
        if (owner.tag.Equals(throwable.ownerTag)) return;
        Hurt();
    }

    public void Hurt()
    {
        onHurt?.Invoke();
    }
}