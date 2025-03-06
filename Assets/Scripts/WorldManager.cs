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
    [HideInInspector] public GoalDiamond diamond;

    private void Start()
    {
        if (world == null) world = FindObjectOfType<Tilemap>();
    }

    public TileBase GetTile(Vector3 worldPosition)
    {
        var cell = world.WorldToCell(worldPosition);
        return world.GetTile(cell);
    }

    public List<Vector3> FindPath(Vector3 start, Vector3 end, int depth = 200)
    {
        var path = new List<Vector3>();

        if (!world) return path;

        var startTile = world.WorldToCell(start);
        var endTile = world.WorldToCell(end);

        if (world.GetTile(startTile)) return path;
        if (world.GetTile(endTile)) return path;
        Debug.Log($"Finding path from {startTile} to {endTile}");

        return AStar.Search(startTile, endTile, AStar.CrossProduct, depth)
            .Select(it => world.CellToWorld(it) + new Vector3(0.5f, 0.5f))
            .ToList();
    }
}