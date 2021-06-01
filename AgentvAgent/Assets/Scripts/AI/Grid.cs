using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Grid : MonoBehaviour
{
    Node[,] grid;

    public bool displayGridGizmos;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkableMask;

    int wallWeight = 2;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    public Node RequestNode(Node node)
    {
        return grid[node.gridX, node.gridY];
    }

    // Takes a GridUpdate, adding the weighting to the grid node and 
    // surrounding nodes with weaker weightings.
    public void UpdateNodeWeight(GridUpdate update)
    {
        lock (grid)
        {
            grid[update.nodeToUpdate.gridX, update.nodeToUpdate.gridY].movementWeight += update.weight;
            foreach (Node n in this.GetNeighbours(update.nodeToUpdate))
            {
                grid[n.gridX, n.gridY].movementWeight += (update.weight / 2);
                foreach (Node m in this.GetNeighbours(n))
                {
                    grid[m.gridX, m.gridY].movementWeight += (update.weight / 4);
                }
            }

        }
    }

    // Creates a X by Y grid, size specified in the inspector, of nodes.
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y, 0);
            }
        }
        AddWallWeights();
    }

    // For each node surrounding an unwalkable node, add a slight
    // weighting so that AI don't get too close to obstructions.
    void AddWallWeights()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                List<Node> neighbours = GetNeighbours(grid[x, y]);
                foreach (Node n in neighbours)
                {
                    if (!n.walkable)
                    {
                        grid[x, y].movementWeight += wallWeight;
                        foreach (Node m in this.GetNeighbours(n))
                        {
                            m.movementWeight += wallWeight / 2;
                        }
                        break;
                    }
                }
            }
        }
    }

    // For a given input node, return a list of all surrounding nodes
    // from the grid.
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    // From a given world point, convert it into a grid point and
    // return the node that corresponds to that point if it exists.
    public Node WorldPosToNode(Vector3 posToNode)
    {
        float percentX = (posToNode.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (posToNode.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    // From a given WorldPosToNode, convert it into a grid point and
    // callback the node that corresponds to that point if it exists.
    public void WorldPosToNode(WorldPosToNode posToNode)
    {
        float percentX = (posToNode.worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (posToNode.worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        bool nodeExists = (grid[x, y] == null) ? false : true;
        posToNode.callback(grid[x, y], nodeExists);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                int nodeWeight = n.movementWeight;
                Color weight = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(-8, 8, nodeWeight));
                Gizmos.color = (n.walkable) ? weight : Color.red;
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
