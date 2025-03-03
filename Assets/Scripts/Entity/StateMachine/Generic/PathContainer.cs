using System.Collections.Generic;
using UnityEngine;

public class PathContainer: AbstractTargetContainer
{
    private List<Vector3> _path = new();
    private int _index = 0;

    public void SetPath(List<Vector3> path)
    {
        _path = path;
        _index = path.Count > 1 ? 1 : 0;
    }

    public bool NextPoint()
    {
        _index++;
        return _index < _path.Count;
    }
    
    public override Vector3 GetLocation()
    {
        if (_index >= _path.Count) return transform.position;
        return _path[_index];
    }
}