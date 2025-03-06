using UnityEngine;

public class SoundDetector : MonoBehaviour
{
    [SerializeField] private EventTrigger trigger;
    [SerializeField] private LocationContainer targetContainer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Sound")) return;
        targetContainer.UpdatePosition(other.transform.position);
        trigger.TriggerEvent();
    }

    public void SetLocation(AbstractTargetContainer container)
    {
        targetContainer.UpdatePosition(container.GetLocation());
    }
}