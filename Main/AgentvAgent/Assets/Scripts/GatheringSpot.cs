using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringSpot
{
    Spot[] spots;

    bool checkStatus()
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

    Vector3 assignSpot()
    {
        Spot spot = null;
        foreach (Spot s in spots)
        {
            if (s.getOccupation() == false)
            {
                spot = s;
            }
        }
        spot.changeOccupation();
        return spot.location;
    }
}
