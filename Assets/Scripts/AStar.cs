using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class AStar : MonoBehaviour
{
    public static Stack<Vector3Int> Search(
        Vector3Int start,
        Vector3Int end,
        Heuristic heuristic, int maxDepth = 1000000)
    {
        #region initialization

        var currentDepth = 0;
        var path = new Stack<Vector3Int>();

        var startRecord = new NodeRecord
        {
            Tile = start,
            CostSoFar = 0f,
            PreviousRecord = null,
            EstimatedTotalCost = heuristic(start, start, end)
        };
        var open = new List<NodeRecord> { startRecord };
        var closed = new List<NodeRecord>();

        var current = new NodeRecord();

        #endregion

        while (open.Count > 0 && currentDepth < maxDepth)
        {
            currentDepth++;
            current = GetSmallest(open);

            var hasReachedEnd = current.Tile == end;
            if (hasReachedEnd) break;

            var connections = current.GetConnections();

            foreach (var nextNode in connections)
            {
                var nextNodeCost = current.CostSoFar + 1f;
                float nextNodeHeuristic;

                NodeRecord nextRecord;

                var haveExploredNode = HasNode(closed, nextNode);
                if (haveExploredNode)
                {
                    nextRecord = closed.First(record => record.Tile == nextNode);

                    var nextIsWorseThanPrevious = nextRecord.CostSoFar <= nextNodeCost;
                    if (nextIsWorseThanPrevious) continue;

                    closed.Remove(nextRecord);

                    nextNodeHeuristic = nextRecord.EstimatedTotalCost - nextRecord.CostSoFar;
                }
                else if (HasNode(open, nextNode))
                {
                    nextRecord = open.First(record => record.Tile == nextNode);

                    var nextIsWorseThanPrevious = nextRecord.CostSoFar <= nextNodeCost;
                    if (nextIsWorseThanPrevious) continue;

                    nextNodeHeuristic = nextRecord.EstimatedTotalCost - nextRecord.CostSoFar;
                }
                else
                {
                    nextRecord = new NodeRecord
                    {
                        Tile = nextNode
                    };
                    nextNodeHeuristic = heuristic(start, nextNode, end);
                }

                nextRecord.CostSoFar = nextNodeCost;
                nextRecord.PreviousRecord = current;
                nextRecord.EstimatedTotalCost = nextNodeHeuristic + nextNodeCost;

                var isNodeInFrontier = HasNode(open, nextNode);
                if (!isNodeInFrontier)
                {
                    open.Add(nextRecord);
                }
            }

            open.Remove(current);
            closed.Add(current);
        }

        // Determine whether A* found a path and print it here.
        var pathDidNotReachEnd = current.Tile != end;
        if (pathDidNotReachEnd)
        {
            Debug.Log("Search Failed");
            return new Stack<Vector3Int>();
        }

        while (current != null)
        {
            path.Push(current.Tile);
            current = current.PreviousRecord;
        }

        return path;
    }

    private static NodeRecord GetSmallest(List<NodeRecord> records)
    {
        var currentMin = records[0];
        foreach (var record in records)
        {
            if (record.EstimatedTotalCost < currentMin.EstimatedTotalCost)
            {
                currentMin = record;
            }
        }

        return currentMin;
    }

    private static bool HasNode(List<NodeRecord> records, Vector3Int tile)
    {
        return records.Any(record => record.Tile == tile);
    }

    public delegate float Heuristic(Vector3Int start, Vector3Int tile, Vector3Int goal);

    public static float Uniform(Vector3Int start, Vector3Int tile, Vector3Int goal)
    {
        return 0f;
    }

    public static float Manhattan(Vector3Int start, Vector3Int tile, Vector3Int goal)
    {
        var dx = Math.Abs(tile.x - goal.x);
        var dy = Math.Abs(tile.y - goal.y);
        return dx + dy;
    }

    public static float CrossProduct(Vector3Int start, Vector3Int tile, Vector3Int goal)
    {
        var dx1 = tile.x - goal.x;
        var dy1 = tile.y - goal.y;

        var dx2 = start.x - goal.x;
        var dy2 = start.y - goal.y;

        var crossProduct = Math.Abs(dx1 * dy2 - dx2 * dy1);
        return Manhattan(start, tile, goal) + crossProduct * 0.001f;
    }
}