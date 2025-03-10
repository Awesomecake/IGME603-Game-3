using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using System.Linq;
//using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class ProcGenBehavior : MonoBehaviour
{
    private uint numHorizontalChunks = 5;
    private uint numVerticalChunks = 5;
    private uint chunkWidth = 10;
    private uint chunkHeight = 10;

    private uint terrainWidth = 0;
    private uint terrainHeight = 0;

    [Header("General Generation Vars")]
    [SerializeField] private uint seed = 0;
    private float noiseAmplitude = 0.5f;
    private float magnification = 1f;
    [SerializeField] private uint boarderWidth = 10;
    [SerializeField] private SeedManager seedManager;

    [Header("Random Walker Vars")]
    [SerializeField] private uint minWalkerDistance = 7;
    [SerializeField] private uint maxWalkerDistance = 14;
    private List<Vector2Int> walkerPath;

    [Header("Player/Guard/LilBro/Gem Vars")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject guardPrefab;
    [SerializeField] private GameObject lilBroPrefab;
    [SerializeField] private GameObject gemPrefab;
    private bool hasPlayerSpawned = false;
    private bool hasLilBroSpawned = false;
    private bool hasGemSpawned = false;

    private GameObject currentPlayer;
    private GameObject currentLilBro;

    private enum Direction
    {
        LEFT, RIGHT, DOWN, UP
    };

    private enum WalkerValue
    {
        OPEN, STEPPED, CLOSED
    };

    private enum ChunkValue
    {
        FILLED, LEFT_RIGHT, LEFT_UP, LEFT_DOWN, UP_DOWN, RIGHT_UP, RIGHT_DOWN,
        MIDDLE_LEFT, MIDDLE_UP, MIDDLE_DOWN, MIDDLE_RIGHT,
        PLAYER_LEFT, PLAYER_RIGHT, PLAYER_UP, PLAYER_DOWN,
        GEM_LEFT, GEM_RIGHT, GEM_UP, GEM_DOWN,
        LB_LEFT_RIGHT, LB_LEFT_UP, LB_LEFT_DOWN, LB_UP_DOWN, LB_RIGHT_UP, LB_RIGHT_DOWN
    };

    [Header("Gameobject Vars")]
    [SerializeField] private TileBase wallTile;
    [SerializeField] private TileBase playerTile;
    [SerializeField] private TileBase guardTile;
    [SerializeField] private TileBase lilBroTile;
    [SerializeField] private TileBase gemTile;
    [SerializeField] private Tilemap wallTimeMap;
    [SerializeField] private TileBase openGroundTile;
    [SerializeField] private TileBase closedGroundTile;
    [SerializeField] private Tilemap groundTileMap;

    private WalkerValue[,] walkerMap;
    private ChunkValue[,] chunkMap;
    private int[,] map;

    [Header("Module Vars")]
    [SerializeField] private List<GameObject> NONEModules;
    [SerializeField] private List<GameObject> LEFT_RIGHTModules;
    [SerializeField] private List<GameObject> LEFT_UPModules;
    [SerializeField] private List<GameObject> LEFT_DOWNModules;
    [SerializeField] private List<GameObject> UP_DOWNModules;
    [SerializeField] private List<GameObject> RIGHT_UPModules;
    [SerializeField] private List<GameObject> RIGHT_DOWNModules;
    [SerializeField] private List<GameObject> MIDDLE_LEFTModules;
    [SerializeField] private List<GameObject> MIDDLE_UPModules;
    [SerializeField] private List<GameObject> MIDDLE_DOWNModules;
    [SerializeField] private List<GameObject> MIDDLE_RIGHTModules;

    [Header("Player Module Vars")]
    [SerializeField] private List<GameObject> PLAYER_LEFTModules;
    [SerializeField] private List<GameObject> PLAYER_UPModules;
    [SerializeField] private List<GameObject> PLAYER_RIGHTModules;
    [SerializeField] private List<GameObject> PLAYER_DOWNModules;

    [Header("Lil Bro Module Vars")]
    [SerializeField] private List<GameObject> LB_LEFT_RIGHTModules;
    [SerializeField] private List<GameObject> LB_LEFT_UPModules;
    [SerializeField] private List<GameObject> LB_LEFT_DOWNModules;
    [SerializeField] private List<GameObject> LB_UP_DOWNModules;
    [SerializeField] private List<GameObject> LB_RIGHT_UPModules;
    [SerializeField] private List<GameObject> LB_RIGHT_DOWNModules;

    [Header("gem Module Vars")]
    [SerializeField] private List<GameObject> GEM_LEFTModules;
    [SerializeField] private List<GameObject> GEM_UPModules;
    [SerializeField] private List<GameObject> GEM_RIGHTModules;
    [SerializeField] private List<GameObject> GEM_DOWNModules;

    void Start()
    {
        //if seedManager is to use the storedSeed,...
        if (seedManager.useSeed)
        {
            //use the stored seed to generate the level
            seed = seedManager.storedSeed;
        }
        //else seedManager says to NOT use the current seed
        else
        {
            //use a new random seed to generate the level
            seed = (uint)UnityEngine.Random.Range(0f, 4294967295f);
        }

        //reset useSeed
        seedManager.useSeed = false;

        //Debug.Log("ProcGen | useSeed = " + seedManager.useSeed + ".\n And currently used seed = " + seed +"\n");

        //generate level based on seed
        UnityEngine.Random.InitState((int)seed);

        //calculate terrainWidth and terrainHeight
        terrainWidth = numHorizontalChunks * chunkWidth;
        terrainHeight = numVerticalChunks * chunkHeight;

        //if wallTile and wallTimeMap have been assigned,..
        if (wallTile != null && wallTimeMap != null)
        {
            //generate terrain at the start of the scene
            //Generate();
            GenerateChunks();
        }

        CameraManager.Instance.TrackPlayer(currentPlayer.transform);
        LevelManager.Instance.lilBro = currentLilBro;
    }

    //***** VALUE AND TERRAIN CHANGE AT RUNTIME FUNCTIONS *****
    void Update()
    {
        //Debug key for testing generation REMOVE WHEN PUSHING/BUILDING
        if (Input.GetKeyDown(KeyCode.R))
        {
            //if wallTile and wallTimeMap have been assigned,..
            if (wallTile != null && wallTimeMap != null)
            {
                //generate terrain at the start of the scene
                //Generate();
                //GenerateChunks();
                if (LevelManager.Instance)
                {
                    LevelManager.Instance.ReloadScene();
                }
                else
                {
                    //reset useSeed
                    seedManager.useSeed = false;

                    //reload scene
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
    }

    public void ChangeMagnitude(float newMag)
    {
        magnification = newMag;
    }

    public uint GetSeed()
    {
        //return the current seed
        return seed;
    }

    public void SaveSeed()
    {
        //save the current seed into seedManager
        seedManager.storedSeed = GetSeed();

        //set useSeed to true
        seedManager.useSeed = true;
    }

    //***** CHUNK AND RANDOM WALKER FUNCTIONS *****
    private void GenerateChunks()
    {
        //*** Proc Gen Setup ***
        //Clear all pre-existing ground tiles
        wallTimeMap.ClearAllTiles();

        //Create an array of chunks to be randomly walked through and populated with modules
        walkerMap = GenerateWalkerkMap(numHorizontalChunks, numVerticalChunks, WalkerValue.OPEN);

        //Create a list of Vector2Int to populate with the walker's steps
        walkerPath = new List<Vector2Int>();

        //Create an array of chunks to be populated with modules
        chunkMap = GenerateChunkMap(numHorizontalChunks, numVerticalChunks, ChunkValue.FILLED);

        //Create a 2D map of 0's to manipulate and render later
        map = GenerateMap(terrainWidth, terrainHeight, 0);

        //*** Generate ***
        //randomly walk through the walkerMap to generate a walkerPath
        walkerPath = RandomWalk(walkerMap, walkerPath);

        //Debug.Log("ProcGen | Final path was of length = " + walkerPath.Count);
        //for (int i = 0; i < walkerPath.Count; i++)
        //{
        //    Debug.Log(walkerPath[i]);
        //}

        //assign chunk values for each chunk based on the walkerPath
        chunkMap = AssignChunkValues(chunkMap, walkerPath);

        //
        chunkMap = AssignSpecialChunkValues(chunkMap, walkerPath);

        //populate the chunkMap with modules based on their chunk values
        map = PlaceModules(chunkMap, chunkWidth, chunkHeight, map);

        //*** Render ***
        //Render a tilemap using the array
        RenderMap(map, wallTimeMap, wallTile);

        //Generate and render a boarder of width = boarderWidth around the rendered map
        GenerateAndRenderBoarder(boarderWidth, wallTimeMap, wallTile, groundTileMap, closedGroundTile);
    }

    private ChunkValue[,] GenerateChunkMap(uint numHorizontalChunks, uint numVerticalChunks, ChunkValue initValue)
    {
        //create 2D integer array of numHorizontalChunks = numHorizontalChunks and numVerticalChunks = numVerticalChunks
        ChunkValue[,] chunkMap = new ChunkValue[numHorizontalChunks, numVerticalChunks];

        //iterate through numHorizontalChunks and numVerticalChunks of map
        for (int x = 0; x < numHorizontalChunks; x++)
        {
            for (int y = 0; y < numVerticalChunks; y++)
            {
                //set chunkMap[x,y] to initValue
                chunkMap[x, y] = initValue;
            }
        }

        //return map
        return chunkMap;
    }

    private WalkerValue[,] GenerateWalkerkMap(uint numHorizontalChunks, uint numVerticalChunks, WalkerValue initValue)
    {
        //create 2D integer array of numHorizontalChunks = numHorizontalChunks and numVerticalChunks = numVerticalChunks
        WalkerValue[,] walkerMap = new WalkerValue[numHorizontalChunks, numVerticalChunks];

        //iterate through numHorizontalChunks and numVerticalChunks of map
        for (int x = 0; x < numHorizontalChunks; x++)
        {
            for (int y = 0; y < numVerticalChunks; y++)
            {
                //set walkerMap[x,y] to initValue
                walkerMap[x, y] = initValue;
            }
        }

        //return walkerMap
        return walkerMap;
    }

    private List<Vector2Int> RandomWalk(WalkerValue[,] walkerkMap, List<Vector2Int> walkerPath)
    {
        //define currentWalkerDistance
        uint currentWalkerDistance = 1;

        //find a random starting position for the walker
        Vector2Int currentPosition = new Vector2Int(UnityEngine.Random.Range(0, (int)numHorizontalChunks - 1), UnityEngine.Random.Range(0, (int)numHorizontalChunks  - 1));
        //Debug.Log("Current position is: " + currentPosition);

        //set starting position to WalkerValue.STEPPED
        walkerkMap[currentPosition.x, currentPosition.y] = WalkerValue.STEPPED;

        //add step to walkerPath
        walkerPath.Add(currentPosition);

        //define all possible next steps
        List<Vector2Int> possibleNextSteps = new List<Vector2Int>();
        possibleNextSteps = FindPossibleSteps(walkerkMap, currentPosition);

        //for(int j = 0; j < possibleNextSteps.Count; j++)
        //{
        //    Debug.Log("Possible Direction #" + j + " can go to: " + possibleNextSteps[j]);
        //}

        int maxWalkAttempts = 100;
        int curWalkAttempts = 0;

        //randomly walk through chunkMap UNTIL maxWalkerDistance is reached 
        // && possibleNextSteps.Count != 0
        while (currentWalkerDistance < maxWalkerDistance)
        {
            //safety break
            if (curWalkAttempts >= maxWalkAttempts)
                break;

            //if there is at least 1 possible next step,...
            if (possibleNextSteps.Count != 0)
            {
                //step in a random (but possible) direction
                currentPosition = possibleNextSteps[UnityEngine.Random.Range(0, possibleNextSteps.Count - 1)];

                //increment currentWalkerDistance
                currentWalkerDistance++;

                //set current position on walkerkMap to WalkerValue.STEPPED
                walkerkMap[currentPosition.x, currentPosition.y] = WalkerValue.STEPPED;

                //add step to walkerPath
                walkerPath.Add(currentPosition);

                //update possibleNextSteps
                possibleNextSteps = FindPossibleSteps(walkerkMap, currentPosition);
            }
            //else there are NO possible next steps,...
            else
            {
                //Debug.Log("ProcGen | Starting to Prune! Current path length = " + walkerPath.Count);
                //then prune by walking backwards until you reach a chunk that has at least 1 possible direction
                while (possibleNextSteps.Count == 0 && walkerPath.Count > 2)
                {
                    //Debug.Log("ProcGen | Stepped Backwards");
                    //set current position on walkerkMap to WalkerValue.CLOSED so that the walker never returns here
                    walkerkMap[currentPosition.x, currentPosition.y] = WalkerValue.CLOSED;

                    //step backwards
                    currentPosition = walkerPath[walkerPath.Count - 2];

                    //decrement currentWalkerDistance
                    currentWalkerDistance--;

                    //set current position on walkerkMap to WalkerValue.STEPPED
                    walkerkMap[currentPosition.x, currentPosition.y] = WalkerValue.STEPPED;

                    //remove last step from walkerPath
                    walkerPath.RemoveAt(walkerPath.Count - 1);

                    //update possibleNextSteps
                    possibleNextSteps = FindPossibleSteps(walkerkMap, currentPosition);

                    //increment curWalkAttempts
                    curWalkAttempts++;
                }
                //Debug.Log("ProcGen | Done Pruning! Current path length = " + walkerPath.Count);
            }

            //increment curWalkAttempts
            curWalkAttempts++;
        }

        //return walkerPath
        return walkerPath;
    }

    private List<Vector2Int> FindPossibleSteps(WalkerValue[,] walkerMap, Vector2Int currentPosition)
    {
        //get numHorizontalChunks and numVerticalChunks
        int numHorizontalChunks = chunkMap.GetLength(0);
        int numVerticalChunks = chunkMap.GetLength(1);

        //define all possible next steps
        List<Vector2Int> possibleNextSteps = new List<Vector2Int>();

        //check left
        //if the chunk to the left is NOT out of bounds,...
        if (currentPosition.x - 1 >= 0)
        {
            //if chunk to the left has NOT been stepped in and is NOT closed,...
            if (walkerMap[currentPosition.x - 1, currentPosition.y] != WalkerValue.STEPPED && walkerMap[currentPosition.x - 1, currentPosition.y] != WalkerValue.CLOSED)
            {
                //add the chunk to the left as a possible direction to step in
                possibleNextSteps.Add(new Vector2Int(currentPosition.x - 1, currentPosition.y));
            }
        }

        //check up
        //if the chunk to the up is NOT out of bounds,...
        if (currentPosition.y + 1 < numVerticalChunks)
        {
            //if chunk to the left has NOT been stepped in and is NOT closed,...
            if (walkerMap[currentPosition.x, currentPosition.y + 1] != WalkerValue.STEPPED && walkerMap[currentPosition.x, currentPosition.y + 1] != WalkerValue.CLOSED)
            {
                //add the chunk to the left as a possible direction to step in
                possibleNextSteps.Add(new Vector2Int(currentPosition.x, currentPosition.y + 1));
            }
        }

        //check right
        //if the chunk to the rught is NOT out of bounds,...
        if (currentPosition.x + 1 < numHorizontalChunks)
        {
            //if chunk to the right has NOT been stepped in and is NOT closed,...
            if (walkerMap[currentPosition.x + 1, currentPosition.y] != WalkerValue.STEPPED && walkerMap[currentPosition.x + 1, currentPosition.y] != WalkerValue.CLOSED)
            {
                //add the chunk to the right as a possible direction to step in
                possibleNextSteps.Add(new Vector2Int(currentPosition.x + 1, currentPosition.y));
            }
        }

        //check down
        //if the chunk to the down is NOT out of bounds,...
        if (currentPosition.y - 1 >= 0)
        {
            //if chunk to the down has NOT been stepped in and is NOT closed,...
            if (walkerMap[currentPosition.x, currentPosition.y - 1] != WalkerValue.STEPPED && walkerMap[currentPosition.x, currentPosition.y - 1] != WalkerValue.CLOSED)
            {
                //add the chunk to the down as a possible direction to step in
                possibleNextSteps.Add(new Vector2Int(currentPosition.x, currentPosition.y - 1));
            }
        }

        //return possibleNextSteps
        return possibleNextSteps;
    }

    private ChunkValue[,] AssignChunkValues(ChunkValue[,] chunkMap, List<Vector2Int> walkerPath)
    {
        //get numHorizontalChunks and numVerticalChunks
        int numHorizontalChunks = chunkMap.GetLength(0);
        int numVerticalChunks = chunkMap.GetLength(1);

        //define currentPosition, lastPosition, and nextPosition
        Vector2Int currentPosition;
        Vector2Int lastPosition;
        Vector2Int nextPosition;

        //iterate through walkerPath
        for (int i = 0; i < walkerPath.Count; i++)
        {
            //set currentPosition
            currentPosition = walkerPath[i];

            //set lastPosition
            if (i == 0)
                lastPosition = currentPosition;
            else
                lastPosition = walkerPath[i - 1];

            //set nextPosition
            if (i == walkerPath.Count - 1)
                nextPosition = currentPosition;
            else
                nextPosition = walkerPath[i + 1];

            //define curToLastDirection and curToNextDirection
            Vector2Int curToLastDirection = lastPosition - currentPosition;
            Vector2Int curToNextDirection = nextPosition - currentPosition;

            //based on curToLastDirection and curToNextDirection, assign chunkMap value to: FILLED, LEFT_RIGHT, LEFT_UP, LEFT_DOWN, UP_DOWN, RIGHT_UP, RIGHT_DOWN, MIDDLE_LEFT, MIDDLE_UP, MIDDLE_DOWN, or MIDDLE_RIGHT
            switch ((curToLastDirection, curToNextDirection))
            {
                //going LEFT_RIGHT
                case var value when value == (Vector2Int.left, Vector2Int.right):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.LEFT_RIGHT;
                    break;
                //going LEFT_RIGHT
                case var value when value == (Vector2Int.right, Vector2Int.left):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.LEFT_RIGHT;
                    break;
                //going LEFT_UP
                case var value when value == (Vector2Int.left, Vector2Int.up):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.LEFT_UP;
                    break;
                //going LEFT_UP
                case var value when value == (Vector2Int.up, Vector2Int.left):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.LEFT_UP;
                    break;
                //going LEFT_DOWN
                case var value when value == (Vector2Int.left, Vector2Int.down):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.LEFT_DOWN;
                    break;
                //going LEFT_DOWN
                case var value when value == (Vector2Int.down, Vector2Int.left):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.LEFT_DOWN;
                    break;
                //going UP_DOWN
                case var value when value == (Vector2Int.up, Vector2Int.down):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.UP_DOWN;
                    break;
                //going UP_DOWN
                case var value when value == (Vector2Int.down, Vector2Int.up):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.UP_DOWN;
                    break;
                //going RIGHT_UP
                case var value when value == (Vector2Int.right, Vector2Int.up):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.RIGHT_UP;
                    break;
                //going RIGHT_UP
                case var value when value == (Vector2Int.up, Vector2Int.right):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.RIGHT_UP;
                    break;
                //going RIGHT_DOWN
                case var value when value == (Vector2Int.right, Vector2Int.down):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.RIGHT_DOWN;
                    break;
                //going RIGHT_DOWN
                case var value when value == (Vector2Int.down, Vector2Int.right):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.RIGHT_DOWN;
                    break;
                //going MIDDLE_LEFT
                case var value when value == (Vector2Int.zero, Vector2Int.left):
                    if (hasPlayerSpawned)
                        chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.MIDDLE_LEFT;
                    else
                        chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.PLAYER_LEFT;
                    break;
                //going MIDDLE_LEFT
                case var value when value == (Vector2Int.left, Vector2Int.zero):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.MIDDLE_LEFT;
                    break;
                //going MIDDLE_UP
                case var value when value == (Vector2Int.zero, Vector2Int.up):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.MIDDLE_UP;
                    break;
                //going MIDDLE_UP
                case var value when value == (Vector2Int.up, Vector2Int.zero):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.MIDDLE_UP;
                    break;
                //going MIDDLE_DOWN
                case var value when value == (Vector2Int.zero, Vector2Int.down):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.MIDDLE_DOWN;
                    break;
                //going MIDDLE_DOWN
                case var value when value == (Vector2Int.down, Vector2Int.zero):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.MIDDLE_DOWN;
                    break;
                //going MIDDLE_RIGHT
                case var value when value == (Vector2Int.zero, Vector2Int.right):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.MIDDLE_RIGHT;
                    break;
                //going MIDDLE_RIGHT
                case var value when value == (Vector2Int.right, Vector2Int.zero):
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.MIDDLE_RIGHT;
                    break;
                //going FILLED or anywhere else
                default:
                    chunkMap[currentPosition.x, currentPosition.y] = ChunkValue.FILLED;
                    break;
            }

            //Debug.Log("Cuurent walker pos: " + currentPosition + ". And is orientation: " + chunkMap[currentPosition.x, currentPosition.y]);
        }

        #region OLD CODE
        //iterate through each chunk in chunkMap
        //for (int x = 0; x < numHorizontalChunks; x++)
        //{
        //    for(int y = 0; y < numVerticalChunks; y++)
        //    {
        //        //if chunk is WalkerValue.OPEN,...
        //        if (chunkMap[x,y] == (int)WalkerValue.OPEN)
        //        {
        //            //set chunk to ChunkValue.FILLED
        //            chunkMap[x, y] = (int)ChunkValue.FILLED;
        //        }
        //        else if (chunkMap[x,y] == (int)WalkerValue.STEPPED)
        //        {
        //            //find left chunk
        //            if (x <= 0 || chunkMap[x - 1, y] == (int)ChunkValue.FILLED || chunkMap[x - 1, y] == (int)WalkerValue.OPEN)
        //            {
        //                //leftPosition = (int)ChunkValue.FILLED;
        //                leftPosition = false;
        //            }
        //            else
        //            {
        //                leftPosition = true;
        //            }

        //            //find up chunk
        //            if(y >= numVerticalChunks - 1 || chunkMap[x, y + 1] == (int)ChunkValue.FILLED || chunkMap[x, y + 1] == (int)WalkerValue.OPEN)
        //            {
        //                upPosition = false;
        //            }
        //            else
        //            {
        //                upPosition = true;
        //            }

        //            //find right chunk
        //            if (x >= numHorizontalChunks - 1 || chunkMap[x + 1, y] == (int)ChunkValue.FILLED || chunkMap[x + 1, y] == (int)WalkerValue.OPEN)
        //            {
        //                rightPosition = false;
        //            }
        //            else
        //            {
        //                rightPosition = true;
        //            }

        //            //find down chunk
        //            if (y <= 0 || chunkMap[x, y - 1] == (int)ChunkValue.FILLED || chunkMap[x, y - 1] == (int)WalkerValue.OPEN)
        //            {
        //                upPosition = false;
        //            }
        //            else
        //            {
        //                upPosition = true;
        //            }

        //            //check surrounding chunks and assign chunk value accordingly
        //            //Remember, that the possible ChunkValues are: FILLED, LEFT_RIGHT, LEFT_UP, LEFT_DOWN, UP_DOWN, RIGHT_UP, RIGHT_DOWN, MIDDLE_LEFT, MIDDLE_UP, MIDDLE_DOWN, MIDDLE_RIGHT
        //            switch ((leftPosition, upPosition, rightPosition, downPosition))
        //            {
        //                case (true, false, false, false):
        //                    chunkMap[x, y] = (int)ChunkValue.MIDDLE_LEFT;
        //                    break;
        //                case (true, true, false, false):
        //                    chunkMap[x, y] = (int)ChunkValue.LEFT_UP;
        //                    break;
        //                case (false, true, true, false):
        //                    chunkMap[x, y] = (int)ChunkValue.RIGHT_UP;
        //                    break;
        //                case (false, false, true, true):
        //                    chunkMap[x, y] = (int)ChunkValue.RIGHT_DOWN;
        //                    break;
        //                case (false, false, false, true):
        //                    chunkMap[x, y] = (int)ChunkValue.MIDDLE_DOWN;
        //                    break;
        //                case (true, false, true, false):
        //                    chunkMap[x, y] = (int)ChunkValue.LEFT_RIGHT;
        //                    break;
        //                case (false, true, false, true):
        //                    chunkMap[x, y] = (int)ChunkValue.UP_DOWN;
        //                    break;
        //                case (true, false, false, true):
        //                    chunkMap[x, y] = (int)ChunkValue.LEFT_DOWN;
        //                    break;
        //                case (false, true, false, false):
        //                    chunkMap[x, y] = (int)ChunkValue.MIDDLE_UP;
        //                    break;
        //                case (false, false, true, false):
        //                    chunkMap[x, y] = (int)ChunkValue.MIDDLE_RIGHT;
        //                    break;
        //                default:
        //                    chunkMap[x, y] = (int)ChunkValue.FILLED;
        //                    break;
        //            }
        //        }
        //    }
        //}
        #endregion

        //return chunkMap
        return chunkMap;
    }

    private ChunkValue[,] AssignSpecialChunkValues(ChunkValue[,] chunkMap, List<Vector2Int> walkerPath)
    {
        //if walker path is long enough to have a player chunk and the player has NOT yet spawned,...
        if(walkerPath.Count >= 1 && !hasPlayerSpawned)
        {
            //set hasPlayerSpawned to true
            hasPlayerSpawned = true;

            //reassign the chunkValue of the first position along the walkerPath to the player chunk with the correct orientation
            switch(chunkMap[walkerPath[0].x, walkerPath[0].y])
            {
                case ChunkValue.MIDDLE_LEFT:
                    chunkMap[walkerPath[0].x, walkerPath[0].y] = ChunkValue.PLAYER_LEFT;
                    break;
                case ChunkValue.MIDDLE_UP:
                    chunkMap[walkerPath[0].x, walkerPath[0].y] = ChunkValue.PLAYER_UP;
                    break;
                case ChunkValue.MIDDLE_RIGHT:
                    chunkMap[walkerPath[0].x, walkerPath[0].y] = ChunkValue.PLAYER_RIGHT;
                    break;
                case ChunkValue.MIDDLE_DOWN:
                    chunkMap[walkerPath[0].x, walkerPath[0].y] = ChunkValue.PLAYER_DOWN;
                    break;
                default:
                    break;
            }
        }

        //if walker path is long enough to have the lil bro chunk and the lil bro has NOT yet spawned,...
        if (walkerPath.Count >= 3 && !hasLilBroSpawned)
        {
            //set hasLilBroSpawned to true
            hasLilBroSpawned = true;

            //define walkerHalfWay
            int walkerHalfWay = (walkerPath.Count - 1) / 2;

            //reassign the chunkValue of the first position along the walkerPath to a lil bro chunk with the correct orientation
            switch (chunkMap[walkerPath[walkerHalfWay].x, walkerPath[walkerHalfWay].y])
            {
                case ChunkValue.LEFT_UP:
                    chunkMap[walkerPath[walkerHalfWay].x, walkerPath[walkerHalfWay].y] = ChunkValue.LB_LEFT_UP;
                    break;
                case ChunkValue.LEFT_RIGHT:
                    chunkMap[walkerPath[walkerHalfWay].x, walkerPath[walkerHalfWay].y] = ChunkValue.LB_LEFT_RIGHT;
                    break;
                case ChunkValue.LEFT_DOWN:
                    chunkMap[walkerPath[walkerHalfWay].x, walkerPath[walkerHalfWay].y] = ChunkValue.LB_LEFT_DOWN;
                    break;
                case ChunkValue.UP_DOWN:
                    chunkMap[walkerPath[walkerHalfWay].x, walkerPath[walkerHalfWay].y] = ChunkValue.LB_UP_DOWN;
                    break;
                case ChunkValue.RIGHT_UP:
                    chunkMap[walkerPath[walkerHalfWay].x, walkerPath[walkerHalfWay].y] = ChunkValue.LB_RIGHT_UP;
                    break;
                case ChunkValue.RIGHT_DOWN:
                    chunkMap[walkerPath[walkerHalfWay].x, walkerPath[walkerHalfWay].y] = ChunkValue.LB_RIGHT_DOWN;
                    break;
                default:
                    break;
            }
        }

        //if walker path is long enough to have the gem chunk and the gem has NOT yet spawned,...
        if (walkerPath.Count >= 2 && !hasGemSpawned)
        {
            //set hasGemSpawned to true
            hasGemSpawned = true;

            //define walkerLength
            int walkerLength = walkerPath.Count - 1;

            //reassign the chunkValue of the first position along the walkerPath to a player chunk with the correct orientation
            switch (chunkMap[walkerPath[walkerLength].x, walkerPath[walkerLength].y])
            {
                case ChunkValue.MIDDLE_LEFT:
                    chunkMap[walkerPath[walkerLength].x, walkerPath[walkerLength].y] = ChunkValue.GEM_LEFT;
                    break;
                case ChunkValue.MIDDLE_UP:
                    chunkMap[walkerPath[walkerLength].x, walkerPath[walkerLength].y] = ChunkValue.GEM_UP;
                    break;
                case ChunkValue.MIDDLE_RIGHT:
                    chunkMap[walkerPath[walkerLength].x, walkerPath[walkerLength].y] = ChunkValue.GEM_RIGHT;
                    break;
                case ChunkValue.MIDDLE_DOWN:
                    chunkMap[walkerPath[walkerLength].x, walkerPath[walkerLength].y] = ChunkValue.GEM_DOWN;
                    break;
                default:
                    break;
            }
        }

        //return chunkMap
        return chunkMap;
    }

    private int[,] PlaceModules(ChunkValue[,] chunkMap, uint chunkWidth, uint chunkHeight, int[,] map)
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
            for (int y = 0; y < numVerticalChunks; y++)
            {
                //update mapX and mapY
                mapX = (int)chunkWidth * x;
                mapY = (int)chunkHeight * y;

                //place a module at each chunk in accordance with the chunk's value
                switch (chunkMap[x, y])
                {
                    case ChunkValue.FILLED:
                        map = PlaceOneModule(NONEModules[UnityEngine.Random.Range(0, NONEModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.LEFT_RIGHT:
                        map = PlaceOneModule(LEFT_RIGHTModules[UnityEngine.Random.Range(0, LEFT_RIGHTModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.LEFT_UP:
                        map = PlaceOneModule(LEFT_UPModules[UnityEngine.Random.Range(0, LEFT_UPModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.LEFT_DOWN:
                        map = PlaceOneModule(LEFT_DOWNModules[UnityEngine.Random.Range(0, LEFT_DOWNModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.UP_DOWN:
                        map = PlaceOneModule(UP_DOWNModules[UnityEngine.Random.Range(0, UP_DOWNModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.RIGHT_UP:
                        map = PlaceOneModule(RIGHT_UPModules[UnityEngine.Random.Range(0, RIGHT_UPModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.RIGHT_DOWN:
                        map = PlaceOneModule(RIGHT_DOWNModules[UnityEngine.Random.Range(0, RIGHT_DOWNModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.MIDDLE_LEFT:
                        map = PlaceOneModule(MIDDLE_LEFTModules[UnityEngine.Random.Range(0, MIDDLE_LEFTModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.MIDDLE_UP:
                        map = PlaceOneModule(MIDDLE_UPModules[UnityEngine.Random.Range(0, MIDDLE_UPModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.MIDDLE_DOWN:
                        map = PlaceOneModule(MIDDLE_DOWNModules[UnityEngine.Random.Range(0, MIDDLE_DOWNModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.MIDDLE_RIGHT:
                        map = PlaceOneModule(MIDDLE_RIGHTModules[UnityEngine.Random.Range(0, MIDDLE_RIGHTModules.Count)], mapX, mapY, map);
                        break;

                    //player module cases
                    case ChunkValue.PLAYER_LEFT:
                        map = PlaceOneModule(PLAYER_LEFTModules[UnityEngine.Random.Range(0, PLAYER_LEFTModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.PLAYER_UP:
                        map = PlaceOneModule(PLAYER_UPModules[UnityEngine.Random.Range(0, PLAYER_UPModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.PLAYER_RIGHT:
                        map = PlaceOneModule(PLAYER_RIGHTModules[UnityEngine.Random.Range(0, PLAYER_RIGHTModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.PLAYER_DOWN:
                        map = PlaceOneModule(PLAYER_DOWNModules[UnityEngine.Random.Range(0, PLAYER_DOWNModules.Count)], mapX, mapY, map);
                        break;

                    //lil bro module cases
                    case ChunkValue.LB_LEFT_UP:
                        map = PlaceOneModule(LB_LEFT_UPModules[UnityEngine.Random.Range(0, LB_LEFT_UPModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.LB_LEFT_RIGHT:
                        map = PlaceOneModule(LB_LEFT_RIGHTModules[UnityEngine.Random.Range(0, LB_LEFT_RIGHTModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.LB_LEFT_DOWN:
                        map = PlaceOneModule(LB_LEFT_DOWNModules[UnityEngine.Random.Range(0, LB_LEFT_DOWNModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.LB_UP_DOWN:
                        map = PlaceOneModule(LB_UP_DOWNModules[UnityEngine.Random.Range(0, LB_UP_DOWNModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.LB_RIGHT_UP:
                        map = PlaceOneModule(LB_RIGHT_UPModules[UnityEngine.Random.Range(0, LB_RIGHT_UPModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.LB_RIGHT_DOWN:
                        map = PlaceOneModule(LB_RIGHT_DOWNModules[UnityEngine.Random.Range(0, LB_RIGHT_DOWNModules.Count)], mapX, mapY, map);
                        break;

                    //gem module cases
                    case ChunkValue.GEM_LEFT:
                        map = PlaceOneModule(GEM_LEFTModules[UnityEngine.Random.Range(0, GEM_LEFTModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.GEM_UP:
                        map = PlaceOneModule(GEM_UPModules[UnityEngine.Random.Range(0, GEM_UPModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.GEM_RIGHT:
                        map = PlaceOneModule(GEM_RIGHTModules[UnityEngine.Random.Range(0, GEM_RIGHTModules.Count)], mapX, mapY, map);
                        break;
                    case ChunkValue.GEM_DOWN:
                        map = PlaceOneModule(GEM_DOWNModules[UnityEngine.Random.Range(0, GEM_DOWNModules.Count)], mapX, mapY, map);
                        break;

                    default:
                        map = PlaceOneModule(NONEModules[UnityEngine.Random.Range(0, NONEModules.Count)], mapX, mapY, map);
                        break;
                }
            }
        }

        //return map
        return map;
    }

    private int[,] PlaceOneModule(GameObject chunkToRender, int moduleStartingX, int moduleStartingY, int[,] map)
    {
        Tilemap chunkTileMap = chunkToRender.GetComponent<Tilemap>();

        for(int x = 0; x < chunkWidth; x++)
        {
            for (int y = 0; y < chunkHeight; y++)
            {
                //if the corresponding chunk tile is a base tile,...
                if (chunkTileMap.GetTile(new Vector3Int(x, y, 0)) == wallTile)
                {
                    //set the corresponding map tile to a base tile
                    map[moduleStartingX + x, moduleStartingY + y] = 1;
                }
                //else the corresponding chunk tile is a player tile,...
                else if(chunkTileMap.GetTile(new Vector3Int(x, y, 0)) == playerTile)
                {
                    //set the corresponding map tile to a player tile
                    map[moduleStartingX + x, moduleStartingY + y] = 2;
                }
                //else the corresponding chunk tile is a guard tile,...
                else if (chunkTileMap.GetTile(new Vector3Int(x, y, 0)) == guardTile)
                {
                    //set the corresponding map tile to a guard tile
                    map[moduleStartingX + x, moduleStartingY + y] = 3;
                }
                //else the corresponding chunk tile is a lil bro tile,...
                else if (chunkTileMap.GetTile(new Vector3Int(x, y, 0)) == lilBroTile)
                {
                    //set the corresponding map tile to a lil bro tile
                    map[moduleStartingX + x, moduleStartingY + y] = 4;
                }
                //else the corresponding chunk tile is a gem tile,...
                else if (chunkTileMap.GetTile(new Vector3Int(x, y, 0)) == gemTile)
                {
                    //set the corresponding map tile to a gem tile
                    map[moduleStartingX + x, moduleStartingY + y] = 5;
                }
                //else the corresponding chunk tile is an air tile,...
                else
                {
                    //set the corresponding map tile to an air tile
                    map[moduleStartingX + x, moduleStartingY + y] = 0;
                }
            }
        }

        //return map
        return map;
    }

    //***** LEGACY PERLIN NOISE AND RANDOM CHISELLER FUNCTIONS *****

    /// <summary>
    /// Generate 2D procedural terrain
    /// </summary>
    private void Generate()
    {
        //Clear all pre-existing ground tiles
        wallTimeMap.ClearAllTiles();

        //Create an array to manipulate later
        map = GenerateMap(terrainWidth, terrainHeight, 0);

        //Generate Terrain by directly applying perlin noise
        map = GenerateTerrain(map);

        //Chisel through the map from the left side to the right side using a random walker
        map = GenerateChiselWalker(map);

        //Render a tilemap using the array
        RenderMap(map, wallTimeMap, wallTile);

        //Generate and render a boarder of width = boarderWidth around the rendered map
        GenerateAndRenderBoarder(boarderWidth, wallTimeMap, wallTile, groundTileMap, closedGroundTile);
    }

    /// <summary>
    /// <para> Input: terrainWidth, terrainHeight of 2D integer array, and whether 2D integer array should be filled with 0s or 1s </para>
    /// <para> Output: a 2D integer array of either 0s or 1s based on the value of empty </para>
    /// </summary>
    /// <param name="terrainWidth"> terrainWidth </param>
    /// <param name="terrainHeight"> terrainHeight of 2D integer array </param>
    /// <param name="empty"> whether 2D integer array should be filled with 0s or 1s </param>
    /// <returns> A 2D integer array of either 0s or 1s based on the value of empty </returns>
    private int[,] GenerateMap(uint terrainWidth, uint terrainHeight, int initValue)
    {
        //create 2D integer array of terrainWidth = terrainWidth and terrainHeight = terrainHeight
        int[,] map = new int[terrainWidth, terrainHeight];

        //iterate through terrainWidth and terrainHeight of map
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = 0; y < terrainHeight; y++)
            {
                //set map[x,y] to initValue
                map[x, y] = initValue;
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
                float rawPerlin = Mathf.Clamp01(Mathf.PerlinNoise((x / magnification) + (float)seed, (y / magnification) + (float)seed));

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
        Vector2 currentTilePos = new Vector2(0, UnityEngine.Random.Range(0, terrainHeight - 1));
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
            Direction chosenDirection = possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Count)];
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
    private void RenderMap(int[,] map, Tilemap wallTimeMap, TileBase groundTileBase)
    {
        //get and store terrainWidth and terrainHeight of map
        int terrainWidth = map.GetLength(0);
        int terrainHeight = map.GetLength(1);

        //iterate through terrainWidth and terrainHeight of map
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = 0; y < terrainHeight; y++)
            {
                //Place wall tiles
                //if value of map at [x,y] is 1,...
                if (map[x, y] == 1)
                {
                    wallTimeMap.SetTile(new Vector3Int(x, y, 0), groundTileBase);
                }
                //if value of map at [x,y] is 2,...
                else if (map[x, y] == 2)
                {
                    //spawn player
                    currentPlayer = Instantiate(playerPrefab, wallTimeMap.GetCellCenterWorld(new Vector3Int(x, y, 0)), Quaternion.identity);
                }
                //if value of map at [x,y] is 3,...
                else if (map[x, y] == 3)
                {
                    //spawn guard
                    Instantiate(guardPrefab, wallTimeMap.GetCellCenterWorld(new Vector3Int(x, y, 0)), Quaternion.identity);
                }
                //if value of map at [x,y] is 4,...
                else if (map[x, y] == 4)
                {
                    //spawn lil bro
                    currentLilBro = Instantiate(lilBroPrefab, wallTimeMap.GetCellCenterWorld(new Vector3Int(x, y, 0)), Quaternion.identity);
                }
                //if value of map at [x,y] is 5,...
                else if (map[x, y] == 5)
                {
                    //spawn gem
                    Instantiate(gemPrefab, wallTimeMap.GetCellCenterWorld(new Vector3Int(x, y, 0)), Quaternion.identity);
                }

                //Place ground tiles
                if (map[x, y] == 1)
                {
                    groundTileMap.SetTile(new Vector3Int(x, y, 0), closedGroundTile);
                }
                else
                {
                    groundTileMap.SetTile(new Vector3Int(x, y, 0), openGroundTile);
                }
            }
        }
    }

    private void GenerateAndRenderBoarder(uint boarderWidth, Tilemap wallTileMap, TileBase wallTileBase, Tilemap groundTileMap, TileBase groundTileBase)
    {
        //get and store terrainWidth and terrainHeight of map
        int terrainWidth = map.GetLength(0);
        int terrainHeight = map.GetLength(1);

        //generate right boarder from (-boarderWidth, -boarderWidth) to (0, terrainHeight + boarderWidth)
        for (int x = (int)(0 - boarderWidth); x < 0; x++)
        {
            for (int y = (int)(0 - boarderWidth); y < terrainHeight + boarderWidth; y++)
            {
                wallTileMap.SetTile(new Vector3Int(x, y, 0), wallTileBase);

                groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTileBase);
            }
        }

        //generate top boarder from (0, terrainHeight) to (terrainWidth, terrainHeight + boarderWidth)
        for (int x = 0; x < terrainHeight; x++)
        {
            for (int y = terrainWidth; y < terrainHeight + boarderWidth; y++)
            {
                wallTileMap.SetTile(new Vector3Int(x, y, 0), wallTileBase);

                groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTileBase);
            }
        }

        //generate left boarder from (terrainWidth, -boarderWidth) to (terrainWidth + boarderWidth, terrainHeight + boarderWidth)
        for (int x = terrainWidth; x < terrainWidth + boarderWidth; x++)
        {
            for (int y = (int)(0 - boarderWidth); y < terrainHeight + boarderWidth; y++)
            {
                wallTileMap.SetTile(new Vector3Int(x, y, 0), wallTileBase);

                groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTileBase);
            }
        }

        //generate bottom boarder from (0, -boarderWidth) to (terrainWidth, 0)
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = (int)(0 - boarderWidth); y < 0; y++)
            {
                wallTileMap.SetTile(new Vector3Int(x, y, 0), wallTileBase);

                groundTileMap.SetTile(new Vector3Int(x, y, 0), groundTileBase);
            }
        }
    }
}
