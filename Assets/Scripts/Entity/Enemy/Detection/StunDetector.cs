using UnityEngine;

public class StunDetector : MonoBehaviour
{
    [SerializeField] private EventTrigger trigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var throwable = other.GetComponent<Throwable>();
        if (!throwable) return;
        if (throwable.ownerTag is "Enemy") return;
        trigger.TriggerEvent();
    }
}