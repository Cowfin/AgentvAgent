using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();

        Node[] waypoints = new Node[0];
        int[] weights = new int[0];
        bool pathSuccess = false;

        Node startNode = grid.WorldPosToNode(request.pathStart);
        Node targetNode = grid.WorldPosToNode(request.pathEnd);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                pathSuccess = true;
                break;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementWeight;
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            weights = AddWaypointWeights(waypoints);
        }
        callback(new PathResult(waypoints, weights, pathSuccess, request.callback));
    }

    Node[] RetracePath(Node startNode, Node endNode)
    {
        Node currentNode = endNode;
        int steps = 0;
        while (currentNode != startNode)
        {
            steps++;
            currentNode = currentNode.parent;
        }
        currentNode = endNode;
        Node[] path = new Node[steps];
        for (int i = steps - 1; i >= 0; i--)
        {
            path[i] = currentNode;
            currentNode = currentNode.parent;
        }
        return path;
    }

    int[] AddWaypointWeights(Node[] waypoints)
    {
        int[] weights = new int[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            weights[i] = waypoints[i].movementWeight;
        }
        return weights;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }
}
