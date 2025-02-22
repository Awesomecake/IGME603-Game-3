using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProcGenBehavior : MonoBehaviour
{
    [Header("Generation vars")]
    [SerializeField] private int terrainWidth = 0;
    [SerializeField] private int terrainHeight = 0;
    [SerializeField] private float noiseAmplitude = 0.5f;
    [SerializeField] private float magnification = 1f;
    [SerializeField] private float seed = 0f;

    [Header("Gameobject Vars")]
    [SerializeField] private TileBase groundTile;
    [SerializeField] private Tilemap groundTileMap;
    private int[,] map;

    void Start()
    {
        //if groundTile and groundTileMap have been assigned,..
        if (groundTile != null && groundTileMap != null)
        {
            //generate terrain at the start of the scene
            Generate();
        }
    }

    void Update()
    {
        //Debug key for testing generation REMOVE WHEN PUSHING/BUILDING
        if (Input.GetKeyDown(KeyCode.G))
        {
            //if groundTile and groundTileMap have been assigned,..
            if (groundTile != null && groundTileMap != null)
            {
                //generate terrain at the start of the scene
                Generate();
            }
        }
    }

    /// <summary>
    /// Generate 2D procedural terrain
    /// </summary>
    private void Generate()
    {
        //Clear all pre-existing ground tiles
        groundTileMap.ClearAllTiles();

        //Create an array to manipulate later
        map = GenerateArray(terrainWidth, terrainHeight, true);

        //Generate Terrain by directly applying perlin noise
        map = GenerateTerrain(map);

        //Render a tilemap using the array
        RenderMap(map, groundTileMap, groundTile);
    }

    /// <summary>
    /// <para> Input: terrainWidth, terrainHeight of 2D integer array, and whether 2D integer array should be filled with 0s or 1s </para>
    /// <para> Output: a 2D integer array of either 0s or 1s based on the value of empty </para>
    /// </summary>
    /// <param name="terrainWidth"> terrainWidth </param>
    /// <param name="terrainHeight"> terrainHeight of 2D integer array </param>
    /// <param name="empty"> whether 2D integer array should be filled with 0s or 1s </param>
    /// <returns> A 2D integer array of either 0s or 1s based on the value of empty </returns>
    private int[,] GenerateArray(int terrainWidth, int terrainHeight, bool empty)
    {
        //create 2D integer array of terrainWidth = terrainWidth and terrainHeight = terrainHeight
        int[,] map = new int[terrainWidth, terrainHeight];

        //iterate through terrainWidth and terrainHeight of map
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = 0; y < terrainHeight; y++)
            {
                //if empty is true, set map[x,y] to 0
                //else if empty is false, set map[x,y] to 1
                map[x, y] = (empty) ? 0 : 1;
            }
        }

        //return map
        return map;
    }

    private int[,] GenerateTerrain(int[,] map)
    {
        //get and store terrainWidth and terrainHeight of map
        int terrainWidth = map.GetLength(0);
        int terrainHeight = map.GetLength(1);

        //iterate through terrainWidth and terrainHeight of map
        for (int x = 0; x < terrainWidth; x++)
        {
            //Debug.Log(Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * terrainHeight / 2));
            //perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * terrainHeight / 2);
            //perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed));
            //perlinHeight += terrainHeight / 2;
            for (int y = 0; y < terrainHeight; y++)
            {
                //generate a raw perlin value for the (x, y) coordinate
                float rawPerlin = Mathf.Clamp01(Mathf.PerlinNoise((x / magnification) + seed, (y / magnification) + seed));

                int intPerlin = 0;

                if (rawPerlin >= Mathf.Clamp01(noiseAmplitude))
                {
                    intPerlin = 1;
                }
                else
                {
                    intPerlin = 0;
                }

                map[x, y] = intPerlin;
            }
        }

        //return map with perlin noise generated terrain
        return map;
    }

    private void RenderMap(int[,] map, Tilemap groundTileMap, TileBase groundTileBase)
    {
        //get and store terrainWidth and terrainHeight of map
        int terrainWidth = map.GetLength(0);
        int terrainHeight = map.GetLength(1);

        //iterate through terrainWidth and terrainHeight of map
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = 0; y < terrainHeight; y++)
            {
                //if value of map at [x,y] is 1,...
                if (map[x, y] == 1)
                {
                    groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTileBase);
                }
            }
        }
    }

    private void GenerateBoarder(int[,] map)
    {

    }
}
