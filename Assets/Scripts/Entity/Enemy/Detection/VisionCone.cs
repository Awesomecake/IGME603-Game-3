using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [SerializeField] private Rigidbody2D self;
    [SerializeField] [Range(2, 100)] private int rayCount = 10;
    [SerializeField] private float angle;
    [SerializeField] private float radius;

    [SerializeField] private MeshFilter filter;
    private Mesh _viewMesh;

    [SerializeField] private PlayerDetector detector;

    private void Start()
    {
        _viewMesh = new Mesh
        {
            name = "VisionCone"
        };
        filter.mesh = _viewMesh;
    }

    private void LateUpdate()
    {
        var didSpot = false;
        var viewPoints = new List<Vector3>();
        var spotted = new List<Transform>();
        var angleStep = angle / (rayCount - 1);
        for (var i = 0; i < rayCount; i++)
        {
            var offset = angle / 2f - angleStep * i;
            var direction = self.transform.up.Rotate(offset - 90f);
            var hit = Physics2D.Raycast(
                origin: self.transform.position,
                direction: direction,
                distance: radius,
                layerMask: LayerMask.GetMask("Wall", "VisionObject")
            );
            var rayPosition = GetHitPosition(hit, direction);

            if (hit.collider && hit.collider.gameObject.layer == LayerMask.NameToLayer("VisionObject"))
            {
                var newHit = Physics2D.Raycast(
                    origin: self.transform.position,
                    direction: direction,
                    distance: radius,
                    layerMask: LayerMask.GetMask("Wall")
                );
                rayPosition = GetHitPosition(newHit, direction);
                detector.SpottedTarget(hit.collider.transform);
                didSpot = true;
            }

            viewPoints.Add(rayPosition);
        }

        if (!didSpot)
        {
            detector.LostAll();
        }
        
        var vertices = new Vector3[viewPoints.Count + 1];
        var triangles = new int[(viewPoints.Count + 1 - 2) * 3];

        vertices[0] = Vector3.zero;
        for (var i = 0; i < viewPoints.Count; i++)
        {
            vertices[i + 1] = self.transform.InverseTransformPoint(viewPoints[i]);

            if (i >= viewPoints.Count - 1) continue;
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        _viewMesh.Clear();

        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;

        _viewMesh.RecalculateNormals();
    }

    private Vector3 GetHitPosition(RaycastHit2D hit, Vector3 direction)
    {
        Vector3 rayPosition;
        if (hit.collider)
        {
            rayPosition = hit.point;
        }
        else
        {
            rayPosition = self.transform.position + direction * radius;
        }

        return rayPosition;
    }
}