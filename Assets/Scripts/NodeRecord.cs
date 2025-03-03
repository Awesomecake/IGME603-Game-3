using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeRecord
{
    private static readonly List<Vector3Int> Offsets = new()
    {
        // new Vector3Int(-1, -1),
        new Vector3Int(-1, 0),
        // new Vector3Int(-1, 1),
        new Vector3Int(0, -1),
        new Vector3Int(0, 1),
        // new Vector3Int(1, -1),
        new Vector3Int(1, 0),
        // new Vector3Int(1, 1),
    };

    public Vector3Int Tile { get; set; }

    public NodeRecord PreviousRecord = null;

    public float CostSoFar { get; set; } = 0f;

    public float EstimatedTotalCost { get; set; } = 0f;

    public List<Vector3Int> GetConnections()
    {
        return Offsets
            .Select(it => it + Tile)
            .Where(it => WorldManager.Instance.world.GetTile(it) == null)
            .ToList();
    }
}