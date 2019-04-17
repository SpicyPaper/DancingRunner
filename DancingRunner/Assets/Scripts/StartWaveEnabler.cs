using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWaveEnabler : MonoBehaviour
{

    public bool IsPlayerInside;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
            IsPlayerInside = true;
        else
            IsPlayerInside = false;
    }
}
