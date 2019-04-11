using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    private float speed;
    private int startHitCounter;

    // Start is called before the first frame update
    void Start()
    {
        speed = 20;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Start")
        {
            if(startHitCounter > 0)
            {
                Destroy(gameObject);
            }
            startHitCounter++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }
}
