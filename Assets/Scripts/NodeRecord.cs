using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeRecord
{
    private static readonly List<Vector3Int> Offsets = new()
    {
        new Vector3Int(-1, 0),
        new Vector3Int(0, -1),
        new Vector3Int(0, 1),
        new Vector3Int(1, 0),
    };

    private static readonly List<Vector3Int> DiagonalOffsets = new()
    {
        new Vector3Int(-1, -1),
        new Vector3Int(-1, 1),
        new Vector3Int(1, -1),
        new Vector3Int(1, 1),
    };

    public Vector3Int Tile { get; set; }

    public NodeRecord PreviousRecord = null;

    public float CostSoFar { get; set; } = 0f;

    public float EstimatedTotalCost { get; set; } = 0f;

    public List<Vector3Int> GetConnections()
    {
        var cardinal = Offsets
            .Select(it => it + Tile)
            .Where(TileIsTraversable);
        
        var diagonal = DiagonalOffsets
            .Where(it => TileIsTraversable(it + Tile))
            .Where(it => TileIsTraversable(new Vector3Int(it.x, 0) + Tile))
            .Where(it => TileIsTraversable(new Vector3Int(0, it.y) + Tile))
            .Select(it => it + Tile);

        return cardinal.Concat(diagonal).ToList();
    }

    private static bool TileIsTraversable(Vector3Int it)
    {
        return WorldManager.Instance.world.GetTile(it) == null;
    }
}