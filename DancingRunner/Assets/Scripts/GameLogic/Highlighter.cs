using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move the attached game object in the game until reaching a game object tagged "start".
/// </summary>
public class Highlighter : MonoBehaviour
{
    private float speed;
    private int startHitCounter;
    public Color color;

    void Start()
    {
        speed = 20;
        gameObject.transform.localScale = new Vector3(100, 100, 0.1f);
    }

    /// <summary>
    /// When a collider trigger, test if the collider is tagged "start".
    /// If yes this game object is destroyed.
    /// </summary>
    /// <param name="collider"></param>
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
    
    /// <summary>
    /// Update the position of this game object
    /// </summary>
    void Update()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }
}
