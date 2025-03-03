using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public UnityEvent onTrigger;

    [SerializeField] private bool isLogging = true;

    public void TriggerEvent()
    {
        if (isLogging) Debug.Log($"triggering {gameObject.name}");
        onTrigger?.Invoke();
    }
}