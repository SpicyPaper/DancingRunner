using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform spawn;
    public Transform player;
    public Transform lava;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y < lava.position.y)
        {
            player.position = spawn.position + Vector3.up * 1.5f;
            rb.velocity = Vector3.zero;
        }
    }
}
