using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnActivatorDetector : MonoBehaviour
{
    private StartWaveEnabler[] startWaveEnablers;
    private void Start()
    {
        startWaveEnablers = GetComponentsInChildren<StartWaveEnabler>();
    }
    

    public bool MustShowAllPlatforms()
    {
        bool showAllPlatforms = true;
        foreach (StartWaveEnabler s in startWaveEnablers)
        {
            if (!s.IsPlayerInside)
            {
                showAllPlatforms = false;
            }
        }
        return showAllPlatforms;
    }
}
