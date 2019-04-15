using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior
{
    private Transform camera;
    private GameObject level;
    private Transform player1;
    private Transform player2;
    private float smoothTime;
    
    private List<GameObject> stages;
    private Vector3 velocity;
    private Vector3 initCamera;

    public CameraBehavior(Transform camera, GameObject level, Transform player1, Transform player2, float smoothTime)
    {
        this.camera = camera;
        this.level = level;
        this.player1 = player1;
        this.player2 = player2;
        this.smoothTime = smoothTime;

        stages = new List<GameObject>();
        foreach (Transform child in level.transform)
        {
            if (child.name.Contains("Stage"))
            {
                stages.Add(child.gameObject);
            }
        }
        initCamera = new Vector3(camera.position.x, camera.position.y, camera.position.z);
    }

    // Update is called once per frame
    public void UpdateCamera()
    {
        int cameraIndex = GetCameraIndex();
        if (cameraIndex != -1)
        {
            PlaceCamera(stages[cameraIndex].transform.Find("Start"), stages[cameraIndex + 1].transform.Find("Start"));
        }
    }

    private void PlaceCamera(Transform left, Transform right)
    {
        float gapPos = (right.position.z + left.position.z) / 2f;
        float gap = right.position.z - left.position.z;
        float distance = (gap - 16f) / 4f;
        float height = distance * Mathf.Tan(Mathf.PI/6);

        camera.position = Vector3.SmoothDamp(camera.position,
                                                new Vector3(initCamera.x + distance, initCamera.y + height, gapPos),
                                                ref velocity, 
                                                smoothTime);
    }

    public int GetCameraIndex()
    {
        int index1 = 0;
        int index2 = 0;

        for (int i = 0; i < stages.Count - 1; i++)
        {
            if (player1.position.z > stages[i].transform.Find("Start").position.z && player1.position.z < stages[i + 1].transform.Find("Start").position.z)
            {
                index1 = i;
            }
            if (player2.position.z > stages[i].transform.Find("Start").position.z && player2.position.z < stages[i + 1].transform.Find("Start").position.z)
            {
                index2 = i;
            }
        }

        if (player1.position.z <= stages[0].transform.Find("Start").position.z)
        {
            index1 = 0;
        }
        if (player2.position.z <= stages[0].transform.Find("Start").position.z)
        {
            index2 = 0;
        }
        if (player1.position.z >= stages[stages.Count - 2].transform.Find("Start").position.z)
        {
            index1 = stages.Count - 2;
        }
        if (player2.position.z >= stages[stages.Count - 2].transform.Find("Start").position.z)
        {
            index2 = stages.Count - 2;
        }
        
        return Mathf.Min(index1, index2);
    }
}
