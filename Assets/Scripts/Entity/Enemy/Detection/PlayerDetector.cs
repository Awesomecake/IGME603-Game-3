using System;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private EventTrigger onEnemySpotted;
    [SerializeField] private EventTrigger onEnemyLost;
    [SerializeField] private MutliTargetContainer targetContainer;

    public void SpottedTarget(Transform target)
    {
        var hadTarget = targetContainer.HasTarget();
        targetContainer.AddTarget(target);
        if (hadTarget) return;
        onEnemySpotted.TriggerEvent();
    }

    public void LostTarget(Transform target)
    {
        var didRemove = targetContainer.RemoveTarget(target);
        if (!didRemove) return;
        if (targetContainer.HasTarget()) return;
        onEnemyLost.TriggerEvent();
    }
}