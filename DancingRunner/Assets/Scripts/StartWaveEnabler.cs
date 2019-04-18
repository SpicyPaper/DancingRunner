using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWaveEnabler : MonoBehaviour
{

    public bool IsPlayerInside;

    /// <summary>
    /// Check whether the player is inside the detector
    /// </summary>
    /// <param name="other">Collider staying inside the detector</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
            IsPlayerInside = true;
        else
            IsPlayerInside = false;
    }
}
