using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;

public class PathRequestManager : MonoBehaviour
{
    public bool drawPathGizmos;
    public List<GatheringSpot> gatheringSpots;

    Queue<PathResult> results = new Queue<PathResult>();
    static PathRequestManager instance;
    Pathfinding pathfinding;
    Grid grid;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        if (results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                for (int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.weights, result.success);
                }
            }
        }
    }

    public static bool DrawPathGizmos()
    {
        return instance.drawPathGizmos;
    }

    public static List<GatheringSpot> RequestGatheringSpots()
    {
        return instance.gatheringSpots;
    }

    public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            instance.pathfinding.FindPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }

    public void FinishedProcessingPath(PathResult result)
    {
        lock (results)
        {
            results.Enqueue(result);
        }
    }

    public static void UpdateGrid(GridUpdate update)
    {
        ThreadStart threadStart = delegate
        {
            instance.grid.UpdateNodeWeight(update);
        };
        threadStart.Invoke();
    }

    public static Node requestNode(Node node)
    {
        return instance.grid.RequestNode(node);
    }

    public static void RequestWorldPosToNode(WorldPosToNode posToNode)
    {
        instance.grid.WorldPosToNode(posToNode);
    }
}
public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Node[], int[], bool> callback;

    public PathRequest(Vector3 _start, Vector3 _end, Action<Node[], int[], bool> _callback)
    {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
    }
}

public struct PathResult
{
    public Node[] path;
    public int[] weights;
    public bool success;
    public Action<Node[], int[], bool> callback;

    public PathResult(Node[] _path, int[] _weights, bool _success, Action<Node[], int[], bool> _callback)
    {
        path = _path;
        weights = _weights;
        success = _success;
        callback = _callback;
    }
}

public struct GridUpdate
{
    public Node nodeToUpdate;
    public int weight;

    public GridUpdate(Node _nodeToUpdate, int _weight)
    {
        nodeToUpdate = _nodeToUpdate;
        weight = _weight;
    }
}

public struct WorldPosToNode
{
    public Vector3 worldPos;
    public Action<Node, bool> callback;

    public WorldPosToNode(Vector3 _worldPos, Action<Node, bool> _callback)
    {
        worldPos = _worldPos;
        callback = _callback;
    }
}