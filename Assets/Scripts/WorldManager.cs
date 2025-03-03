using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Tilemap world;
    public GoalDiamond diamond;

    public List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        var path = new List<Vector3>();

        if (!world) return path;

        var startTile = world.WorldToCell(start);
        var endTile = world.WorldToCell(end);

        if (world.GetTile(startTile) != null) return path;
        if (world.GetTile(endTile) != null) return path;
        Debug.Log($"Finding path from {startTile} to {endTile}");
        
        return AStar.Search(startTile, endTile, AStar.CrossProduct)
            .Select(it => world.CellToWorld(it) + new Vector3(0.5f, 0.5f))
            .ToList();
    }
}