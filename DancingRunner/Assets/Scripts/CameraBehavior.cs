using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject level;
    public Transform player;
    public float smoothTime;
    
    private List<GameObject> stages;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        stages = new List<GameObject>();
        foreach (Transform child in level.transform)
        {
            if (child.name.Contains("Stage"))
            {
                stages.Add(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        int cameraIndex = GetCameraIndex();
        if (cameraIndex != -1)
        {
            PlaceCamera(stages[cameraIndex].transform.Find("Start"), stages[cameraIndex + 1].transform.Find("Start"));
        }
    }

    void PlaceCamera(Transform left, Transform right)
    {
        transform.position = Vector3.SmoothDamp(transform.position,
                                                new Vector3((right.position.x + left.position.x) / 2, transform.position.y, transform.position.z),
                                                ref velocity, 
                                                smoothTime);
    }

    int GetCameraIndex()
    {
        for (int i = 0; i < stages.Count - 1; i++)
        {
            if (player.position.x > stages[i].transform.Find("Start").position.x && player.position.x < stages[i + 1].transform.Find("Start").position.x)
            {
                return i;
            }
        }

        if (player.position.x <= stages[0].transform.Find("Start").position.x)
        {
            return 0;
        }

        return -1;
    }
}
