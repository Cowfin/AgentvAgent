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

    public bool getOccupation()
    {
        return occupied;
    }
    public void changeOccupation()
    {
        occupied = !occupied;
    }

    public void occupationEmpty()
    {
        occupied = false;
    }

    public void occupationFull()
    {
        occupied = true;
    }

}
