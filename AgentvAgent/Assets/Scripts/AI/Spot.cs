using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    public Vector3 location;
    public bool occupied;

    void Start()
    {
        location = this.transform.position;
        occupied = false;
    }

    // Return whether or not this spot is occupied.
    public bool getOccupation()
    {
        return occupied;
    }

    // Set this spots occupation to false.
    public void occupationEmpty()
    {
        occupied = false;
    }

    // Set this spots occupation to true.
    public void occupationFull()
    {
        occupied = true;
    }
}
