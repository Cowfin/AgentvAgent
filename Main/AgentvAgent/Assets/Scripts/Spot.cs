using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    public Vector3 location;
    bool occupied;

    void Start()
    {
        location = this.transform.position;
        bool occupied = false;
    }

    public bool getOccupation()
    {
        return occupied;
    }
    public void changeOccupation()
    {
        occupied = !occupied;
    }
}
