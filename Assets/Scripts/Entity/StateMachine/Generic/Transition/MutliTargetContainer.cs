using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MutliTargetContainer : AbstractTargetContainer
{
    public enum Mode
    {
        Nearest,
        Oldest,
        Newest
    }

    [SerializeField] private Mode mode = Mode.Newest;

    private readonly HashSet<Transform> _targets = new();
    private Vector3 _lastKnownPosition = new Vector3();

    public bool HasTarget()
    {
        return _targets.Count > 0;
    }

    public void AddTarget(Transform newTarget)
    {
        _targets.Add(newTarget);
    }

    public bool RemoveTarget(Transform oldTarget)
    {
        return _targets.Remove(oldTarget);
    }

    public void Clear()
    {
        _targets.Clear();
    }

    public override Vector3 GetCurrentTarget()
    {
        var target = GetTransform();
        if (target)
        {
            _lastKnownPosition = target.position;
        }
        return _lastKnownPosition;
    }

    private Transform GetTransform()
    {
        switch (mode)
        {
            case Mode.Nearest:
                var currentPosition = transform.position;
                var bestDistance = float.MaxValue;
                Transform bestTarget = null;
                
                foreach (var target in _targets)
                {
                    var targetDistance = currentPosition.DistanceTo2DSquared(target.position);
                    if (targetDistance >= bestDistance) continue;
                    bestDistance = targetDistance;
                    bestTarget = target;
                }

                return bestTarget;
            case Mode.Oldest:
                return _targets.FirstOrDefault();
            case Mode.Newest:
                return _targets.LastOrDefault();
            default:
                return null;
        }
    }
}