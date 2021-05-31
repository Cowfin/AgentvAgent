using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAbility : MonoBehaviour
{
    public static bool CheckSpyInRange(float abilityRange, Vector3 hunterPos, Vector3 spyPos)
    {
        if (abilityRange == null || hunterPos == null || spyPos == null)
        {
            return false;
        }
        Vector3 hunterSpyDelta = spyPos - hunterPos;
        float hunterSpyDeltaSquared = hunterSpyDelta.x * hunterSpyDelta.x + hunterSpyDelta.z + hunterSpyDelta.z;
        return (hunterSpyDeltaSquared < (abilityRange * abilityRange));
    }
}
