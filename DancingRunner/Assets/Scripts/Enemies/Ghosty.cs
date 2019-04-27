using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghosty : MonoBehaviour
{
    public float speed;
    public Color color;
    public float horizontalFloatingSpeed;
    public float verticalFloatingSpeed;
    public float horizontalFloatingAmplitude;
    public float verticalFloatingAmplitude;

    private Vector3 initPos;
    private float floatingShift;

    private void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", color);

        GameObject particleSystem = transform.GetChild(0).gameObject;
        particleSystem.GetComponent<Renderer>().material.SetColor("_Color", color);

        initPos = transform.position;
        floatingShift = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
    }

    public float GetFloatingShift()
    {
        return floatingShift;
    }

    public void Respawn()
    {
        transform.position = initPos;
    }
}
