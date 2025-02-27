using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using System.Linq;
//using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProcGenBehavior : MonoBehaviour
{
    [Header("Generation Vars")]
    [SerializeField] private uint numHorizontalChunks = 1;
    [SerializeField] private uint numVerticalChunks = 1;
    [SerializeField] private uint chunkWidth = 5;
    [SerializeField] private uint chunkHeight = 5;
    private uint terrainWidth = 0;
    private uint terrainHeight = 0;
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

    private enum ChunkValue
    { 
        NONE, LEFT_RIGHT, LEFT_UP, LEFT_DOWN, UP_DOWN, RIGHT_UP, RIGHT_DOWN, MIDDLE_LEFT, MIDDLE_UP, MIDDLE_DOWN, MIDDLE_RIGHT
    };

    [Header("Gameobject Vars")]
    [SerializeField] private TileBase groundTile;
    [SerializeField] private Tilemap groundTileMap;
    private int[,] chunkMap;
    private GameObject[,] chunkModules;
    [SerializeField] private List<GameObject> modules;
    private List<int[,]> dataModules;
    private int[,] map;

    [Header("Module Vars")]
    [SerializeField] private List<GameObject> possibleModules;
    [SerializeField] private List<GameObject> NONEModules;

    void Start()
    {
        //calculate terrainWidth and terrainHeight
        terrainWidth = numHorizontalChunks * chunkWidth;
        terrainHeight = numVerticalChunks * chunkHeight;

        //convert all modules to dataModules
        dataModules = ConvertModulesToData(modules);

        //if groundTile and groundTileMap have been assigned,..
        if (groundTile != null && groundTileMap != null)
        {
            //generate terrain at the start of the scene
            //Generate();
            GenerateChunks();
        }
    }

    //***** VALUE AND TERRAIN CHANGE AT RUNTIME FUNCTIONS *****
    void Update()
    {
        //Debug key for testing generation REMOVE WHEN PUSHING/BUILDING
        if (Input.GetKeyDown(KeyCode.G))
        {
            //if groundTile and groundTileMap have been assigned,..
            if (groundTile != null && groundTileMap != null)
            {
                //generate terrain at the start of the scene
                //Generate();
                GenerateChunks();
            }
        }
    }

    public void ChangeMagnitude(float newMag)
    {
        magnification = newMag;
    }

    public void ChangeSeed(float newSeed)
    {
        seed = newSeed;
    }

    //***** CHUNK AND RANDOM WALKER FUNCTIONS *****
    private void GenerateChunks()
    {
        //Clear all pre-existing ground tiles
        groundTileMap.ClearAllTiles();

        //Create an array of chunks to be randomly walked through and populated with modules
        chunkMap = GenerateChunkArray(numHorizontalChunks, numVerticalChunks, false);

        //randomly walk and prune through the chunkMap

        //assign chunk values for each chunk

        //Create an array to manipulate later
        map = GenerateArray(terrainWidth, terrainHeight, true);

        //populate the chunkMap with modules based on their chunk values
        PlaceModules(chunkMap);

        //Render a tilemap using the array
        //RenderChunks(chunkMap, groundTileMap, groundTile);

        //Render a tilemap using the array
        RenderMap(map, groundTileMap, groundTile);

        //Generate and render a boarder of width = boarderWidth around the rendered map
        GenerateAndRenderBoarder(boarderWidth, groundTileMap, groundTile);
    }

    private int[,] GenerateChunkArray(uint numHorizontalChunks, uint numVerticalChunks, bool empty)
    {
        //create 2D integer array of numHorizontalChunks = numHorizontalChunks and numVerticalChunks = numVerticalChunks
        int[,] chunkMap = new int[numHorizontalChunks, numVerticalChunks];

        //iterate through numHorizontalChunks and numVerticalChunks of map
        for (int x = 0; x < numHorizontalChunks; x++)
        {
            for (int y = 0; y < numVerticalChunks; y++)
            {
                //if empty is true, set map[x,y] to 0
                //else if empty is false, set map[x,y] to 1
                chunkMap[x, y] = (empty) ? 0 : (int)ChunkValue.NONE;
            }
        }

        //return map
        return chunkMap;
    }

    private void RenderChunks(int[,] chunkMap, Tilemap groundTileMap, TileBase groundTileBase)
    {
        //get and store terrainWidth and terrainHeight of chunkMap
        uint terrainWidth = numHorizontalChunks * chunkWidth;
        uint terrainHeight = numVerticalChunks * chunkHeight;

        //iterate through terrainWidth and terrainHeight of chunkMap
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = 0; y < terrainHeight; y++)
            {
                //if value of map at [x,y] is 1,...
                if (chunkMap[x, y] == 1)
                {
                    groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTileBase);
                }
            }
        }
    }

    private List<int[,]> ConvertModulesToData(List<GameObject> modules)
    {
        //create a new list
        List<int[,]> dataModules = new List<int[,]>();

        //return dataModules
        return dataModules;
    }

    private void PlaceModules(int[,] chunkMap)
    {
        //get numHorizontalChunks and numVerticalChunks
        int numHorizontalChunks = chunkMap.GetLength(0);
        int numVerticalChunks = chunkMap.GetLength(1);

        //create map indexes
        int mapX = 0;
        int mapY = 0;

        //iterate through chunkMap,...
        for (int x = 0; x < numHorizontalChunks; x++)
        {
            for(int y = 0; y < numVerticalChunks; y++)
            {
                //update mapX and mapY
                mapX = (int)chunkWidth * x;
                mapY = (int)chunkHeight * y;

                //place a module at each chunk in accordance with the chunk's value
                switch (chunkMap[x,y])
                {
                    case (int)ChunkValue.NONE:
                        RenderModule(NONEModules[Random.Range(0, NONEModules.Count - 1)], mapX, mapY);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void RenderModule(GameObject chunkToRender, int moduleStartingX, int moduleStartingY)
    {
        Tilemap chunkTileMap = chunkToRender.GetComponent<Tilemap>();

        for(int x = 0; x < chunkWidth; x++)
        {
            for (int y = 0; y < chunkHeight; y++)
            {
                //if the corresponding chunk tile is a base tile,...
                if (chunkTileMap.GetTile(new Vector3Int(x, y, 0)) == groundTile)
                {
                    map[moduleStartingX + x, moduleStartingY + y] = 1;
                }
                //else the correspionding chunk tile is a air tile,...
                else
                {
                    map[moduleStartingX + x, moduleStartingY + y] = 0;
                }
            }
        }
    }

    //***** PERLIN NOISE AND RANDOM CHISELLER FUNCTIONS *****

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

    //***** RENDER FUNCTIONS *****
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
