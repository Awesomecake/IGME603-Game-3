using System;
using UnityEngine;

public class SoundDetector : MonoBehaviour
{
    [SerializeField] private EventTrigger trigger;
    [SerializeField] private TargetContainer targetContainer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Sound")) return;
        targetContainer.UpdateTarget(other.transform);
        trigger.TriggerEvent();
    }
}