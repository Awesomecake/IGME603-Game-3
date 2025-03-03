using UnityEngine;

public class StunDetector : MonoBehaviour
{
    [SerializeField] private EventTrigger trigger;
    [SerializeField] private string owner = "Enemy";
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var throwable = other.GetComponent<Throwable>();
        if (!throwable) return;
        if (owner.Equals(throwable.ownerTag)) return;
        trigger.TriggerEvent();
    }
}