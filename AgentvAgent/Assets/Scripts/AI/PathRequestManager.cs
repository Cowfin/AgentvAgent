using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    public bool drawPathGizmos;
    public List<GatheringSpot> gatheringSpots;

    Queue<PathResult> results = new Queue<PathResult>();
    static PathRequestManager instance;
    Pathfinding pathfinding;
    PathGrid grid;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
        grid = GetComponent<PathGrid>();
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

    public static void AvailableSpot(SpotRequest request)
    {
        ThreadStart threadStart = delegate
        {
            instance.FindSpot(request, instance.FinishedFindingSpot);
        };
        threadStart.Invoke();
    }

    public void FinishedFindingSpot(SpotResult result)
    {
        result.callback(result.outTarget, true);
    }

    public void FindSpot(SpotRequest request, Action<SpotResult> callback)
    {
        Spot returnSpot = null;
        lock (instance.gatheringSpots)
        {
            List<int> availableGatherings = new List<int>();
            int gatherIndex = 0;
            while (availableGatherings.Count <= 0)
            {
                for (int i = 0; i < instance.gatheringSpots.Count; i++)
                {
                    if (instance.gatheringSpots[i].checkStatus() == false)
                    {
                        //print("ADDED : " + i);

                        availableGatherings.Add(i);
                    }
                }
                gatherIndex = availableGatherings[UnityEngine.Random.Range(0, availableGatherings.Count)];
                //print("Picked : " + availableGatherings[gatherIndex]);
                availableGatherings.Clear();
                for (int j = 0; j < instance.gatheringSpots[gatherIndex].spots.Length; j++)
                {
                    if (instance.gatheringSpots[gatherIndex].spots[j].getOccupation() == false)
                    {
                        if (request.inTarget == null || request.inTarget.location.Equals(instance.gatheringSpots[gatherIndex].spots[j].location) != true)
                        {
                            if (instance.gatheringSpots[gatherIndex].spots[j].getOccupation() == true)
                            {
                                print("ERROR AI PATHING TO SAME SPOT");
                            }
                            availableGatherings.Add(j);
                        }
                    }
                }
            }
            int spotIndex = availableGatherings[UnityEngine.Random.Range(0, availableGatherings.Count)];
            lock (instance.gatheringSpots[gatherIndex].spots[spotIndex])
            {
                returnSpot = instance.gatheringSpots[gatherIndex].spots[spotIndex];
                instance.gatheringSpots[gatherIndex].spots[spotIndex].occupationFull();
            }
        }
        callback(new SpotResult(returnSpot, request.callback));
    }

    public static void releaseSpot(Spot spot)
    {
        lock (instance.gatheringSpots)
        {
            spot.occupationEmpty();
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
    public Spot inTarget;
    public Action<Spot, bool> callback;

    public SpotRequest(Spot _inTarget, Action<Spot, bool> _callback)
    {
        inTarget = _inTarget;
        callback = _callback;
    }
}

public struct SpotResult
{
    public Spot outTarget;
    public Action<Spot, bool> callback;

    public SpotResult(Spot _outTarget, Action<Spot, bool> _callback)
    {
        outTarget = _outTarget;
        callback = _callback;
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