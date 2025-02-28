using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrolArea : MonoBehaviour
{
    [SerializeField] private float width = 5f;
    [SerializeField] private float height = 5f;

    public Vector3 GetRandomLocation()
    {
        var xOffset = Random.Range(-width / 2f, width / 2f);
        var yOffset = Random.Range(-height / 2f, height / 2f);

        return transform.position + new Vector3(xOffset, yOffset, 0f);
    }
    
    private void OnDrawGizmos()
    {
        var points = new[]
        {
            new Vector3(-width / 2f, height / 2f, 0f),
            new Vector3(width / 2f, height / 2f, 0f),
            
            new Vector3(width / 2f, height / 2f, 0f),
            new Vector3(width / 2f, -height / 2f, 0f),
            
            new Vector3(width / 2f, -height / 2f, 0f),
            new Vector3(-width / 2f, -height / 2f, 0f),
            
            new Vector3(-width / 2f, -height / 2f, 0f),
            new Vector3(-width / 2f, height / 2f, 0f),
        };

        Gizmos.color = Color.red;
        Gizmos.DrawLineList(
            new ReadOnlySpan<Vector3>(points)
        );
    }
}