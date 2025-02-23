using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public UnityEvent onTrigger;

    public void TriggerEvent()
    {
        Debug.Log($"triggering {gameObject.name}");
        onTrigger?.Invoke();
    }
}