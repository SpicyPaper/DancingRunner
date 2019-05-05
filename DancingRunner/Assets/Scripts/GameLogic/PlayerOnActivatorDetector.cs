using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script attached to the start in order to check if all platforms must be shown
/// </summary>
public class PlayerOnActivatorDetector : MonoBehaviour
{
    private StartWaveEnabler[] startWaveEnablers;
    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        startWaveEnablers = GetComponentsInChildren<StartWaveEnabler>();
    }
    
    /// <summary>
    /// Return whether or not all platforms must be showed
    /// </summary>
    /// <returns>True if yes, false otherwise</returns>
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
