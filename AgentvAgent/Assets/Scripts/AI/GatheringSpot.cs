using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringSpot : MonoBehaviour
{
    public Spot[] spots;

    // Holds a list of spots, returns true if all spots
    // are full and false othereise.
    public bool checkStatus()
    {
        bool full = true;
        foreach(Spot s in spots)
        {
            if (s.getOccupation() == false)
            {
                full = false;
            }
        }
        return full;
    }
}
