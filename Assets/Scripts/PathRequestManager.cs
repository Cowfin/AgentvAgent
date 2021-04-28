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
    //Queue<SpotRequest> spotReqiests = 
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
        //if()
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

    public static Spot availableSpot(Spot target)
    {
        Spot returnSpot;
        lock (instance.gatheringSpots)
        {
            List<int> availableGatherings = new List<int>();
            List<int> availableSpots = new List<int>();
            int gatherIndex = 0;
            while (availableSpots.Capacity <= 0)
            {
                for (int i = 0; i < instance.gatheringSpots.Capacity; i++)
                {
                    if (instance.gatheringSpots[i].checkStatus() == false)
                    {
                        availableGatherings.Add(i);
                    }
                }
                gatherIndex = availableGatherings[UnityEngine.Random.Range(0, availableGatherings.Capacity - 1)];
                GatheringSpot gathering = instance.gatheringSpots[gatherIndex];
                for (int j = 0; j < instance.gatheringSpots[gatherIndex].spots.Length; j++)
                {
                    if (instance.gatheringSpots[gatherIndex].spots[j].getOccupation() == false)
                    {
                        if (target == null || target.location.Equals(instance.gatheringSpots[gatherIndex].spots[j].location) != true)
                        {
                            availableSpots.Add(j);
                        }
                    }
                }
            }
            int spotIndex = availableSpots[UnityEngine.Random.Range(0, availableSpots.Capacity - 1)];
            returnSpot = instance.gatheringSpots[gatherIndex].spots[spotIndex];
            returnSpot.changeOccupation();
            print("Duplicate " + returnSpot.getOccupation());
            print("Original" + instance.gatheringSpots[gatherIndex].spots[spotIndex].getOccupation());
        }
        return returnSpot;
    }



    public static void releaseSpot(Spot spot)
    {
        lock (spot)
        {
            spot.changeOccupation();
        }
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

public struct SpotRequest
{
    public Spot target;

    public SpotRequest(Spot _target)
    {
        target = _target;
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