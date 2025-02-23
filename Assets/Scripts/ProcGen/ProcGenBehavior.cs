using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProcGenBehavior : MonoBehaviour
{
    [Header("Generation Vars")]
    [SerializeField] private uint terrainWidth = 0;
    [SerializeField] private uint terrainHeight = 0;
    [SerializeField] private float noiseAmplitude = 0.5f;
    [SerializeField] private float magnification = 1f;
    [SerializeField] private float seed = 0f;
    [SerializeField] private uint boarderWidth = 10;

    //[Header("Random Walker Vars")]
    //[SerializeField] private uint maxWalkStraightDistance = 5;
    //private uint currentWalkStraightDistance = 0;
    //[SerializeField] private float turnChancePerStep;
    //private Direction currentDirection = Direction.RIGHT;

    private enum Direction
    {
        RIGHT, DOWN, UP
    };

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

        //Chisel through the map from the left side to the right side using a random walker
        map = GenerateChiselWalker(map);

        //Render a tilemap using the array
        RenderMap(map, groundTileMap, groundTile);

        //Generate and render a boarder of width = boarderWidth around the rendered map
        GenerateAndRenderBoarder(boarderWidth, groundTileMap, groundTile);
    }

    /// <summary>
    /// <para> Input: terrainWidth, terrainHeight of 2D integer array, and whether 2D integer array should be filled with 0s or 1s </para>
    /// <para> Output: a 2D integer array of either 0s or 1s based on the value of empty </para>
    /// </summary>
    /// <param name="terrainWidth"> terrainWidth </param>
    /// <param name="terrainHeight"> terrainHeight of 2D integer array </param>
    /// <param name="empty"> whether 2D integer array should be filled with 0s or 1s </param>
    /// <returns> A 2D integer array of either 0s or 1s based on the value of empty </returns>
    private int[,] GenerateArray(uint terrainWidth, uint terrainHeight, bool empty)
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

    /// <summary>
    /// Chisel through map using a random walker
    /// </summary>
    /// <param name="map"> Map to chisel through </param>
    /// <returns> Map that has been chiselled through by a random walker </returns>
    private int[,] GenerateChiselWalker(int[,] map)
    {
        //get and store terrainWidth and terrainHeight of map
        int terrainWidth = map.GetLength(0);
        int terrainHeight = map.GetLength(1);

        //start at a random point on the leftmost side of the map
        Vector2 currentTilePos = new Vector2(0, Random.Range(0, terrainHeight - 1));
        Direction currentDirection = Direction.RIGHT;
        Debug.Log("Starting tile point of walker = " + currentTilePos);

        //initalize stepNum
        uint stepNum = 0;

        //clear the starting tile
        map[(int)currentTilePos.x, (int)currentTilePos.y] = 0;

        //iterate through terrainWidth
        //for (int x = 0; x < terrainWidth; x++)
        while (currentTilePos.x != terrainWidth - 1 && stepNum <= 200)
        {
            //***** Check all possible positions to go to *****
            List<Direction> possibleDirections = new List<Direction>();
            switch (currentDirection)
            {
                case Direction.RIGHT:
                    possibleDirections.Add(Direction.RIGHT);
                    possibleDirections.Add(Direction.UP);
                    possibleDirections.Add(Direction.DOWN);
                    break;
                case Direction.UP:
                    possibleDirections.Add(Direction.RIGHT);
                    possibleDirections.Add(Direction.UP);
                    break;
                case Direction.DOWN:
                    possibleDirections.Add(Direction.RIGHT);
                    possibleDirections.Add(Direction.DOWN);
                    break;
            }

            //if currentTilePos is at the upper or lower bounds of the map, then remove the posisbilty to pass the bounds
            if (currentTilePos.y >= terrainHeight - 1f)
                possibleDirections.Remove(Direction.UP);
            else if (currentTilePos.y <= 0f)
                possibleDirections.Remove(Direction.DOWN);

            //***** From those possible directions, choose a random one to go in *****
            Direction chosenDirection = possibleDirections[Random.Range(0, possibleDirections.Count)];
            Debug.Log(chosenDirection.ToString());

            //***** Move to the chosen position *****
            switch (chosenDirection)
            {
                case Direction.RIGHT:
                    currentTilePos.x++;
                    break;
                case Direction.UP:
                    currentTilePos.y++;
                    break;
                case Direction.DOWN:
                    currentTilePos.y--;
                    break;
            }

            //clear the moved to tile
            map[(int)currentTilePos.x, (int)currentTilePos.y] = 0;

            //update currentDirection with chosenDirection
            currentDirection = chosenDirection;

            //increment stepNum
            stepNum++;
            Debug.Log("Current stepNum = " + stepNum + ". And position is = " + currentTilePos);
        }

        //clear the last tile
        map[(int)currentTilePos.x, (int)currentTilePos.y] = 0;

        Debug.Log("Ending tile point of walker = " + currentTilePos + ". With a total length = " + stepNum);
        //return map
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

    private void GenerateAndRenderBoarder(uint boarderWidth, Tilemap groundTileMap, TileBase groundTileBase)
    {
        //get and store terrainWidth and terrainHeight of map
        int terrainWidth = map.GetLength(0);
        int terrainHeight = map.GetLength(1);

        //generate right boarder from (-boarderWidth, -boarderWidth) to (0, terrainHeight + boarderWidth)
        for (int x = (int)(0 - boarderWidth); x < 0; x++)
        {
            for (int y = (int)(0 - boarderWidth); y < terrainHeight + boarderWidth; y++)
            {
                groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTileBase);
            }
        }

        //generate top boarder from (0, terrainHeight) to (terrainWidth, terrainHeight + boarderWidth)
        for (int x = 0; x < terrainHeight; x++)
        {
            for (int y = terrainWidth; y < terrainHeight + boarderWidth; y++)
            {
                groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTileBase);
            }
        }

        //generate left boarder from (terrainWidth, -boarderWidth) to (terrainWidth + boarderWidth, terrainHeight + boarderWidth)
        for (int x = terrainWidth; x < terrainWidth + boarderWidth; x++)
        {
            for (int y = (int)(0 - boarderWidth); y < terrainHeight + boarderWidth; y++)
            {
                groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTileBase);
            }
        }

        //generate bottom boarder from (0, -boarderWidth) to (terrainWidth, 0)
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = (int)(0 - boarderWidth); y < 0; y++)
            {
                groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTileBase);
            }
        }
    }
}
