using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;
    public int movementWeight;

    public int gCost;
    public int hCost;
    public Node parent;
    private int heapIndex;

    // Constructor for a node, takes a walkable boolean, world position, grid x and y
    // and also a starting weight.
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _weight)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        movementWeight = _weight;
    }

    // Returns the fCost of this node.
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    // Returns the heapIndex of this node.
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    // Overrides the compare to function so that
    // fCost is the main comparator, followed by
    // hCost.
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
