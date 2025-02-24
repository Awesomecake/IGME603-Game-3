using UnityEngine;

public class StunDetector : MonoBehaviour
{
    [SerializeField] private EventTrigger trigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Projectile")) return;
        trigger.TriggerEvent();
    }
}