using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior
{
    private Transform camera;
    private readonly GameObject level;
    private Transform player1;
    private Transform player2;
    private readonly float smoothTime;
    
    private List<GameObject> milestones;
    private Vector3 velocity;
    private Vector3 initCamera;
    private int maxId;

    public CameraBehavior(Transform camera, GameObject level, Transform player1, Transform player2, float smoothTime)
    {
        this.camera = camera;
        this.level = level;
        this.player1 = player1;
        this.player2 = player2;
        this.smoothTime = smoothTime;

        milestones = new List<GameObject>();
        foreach (Transform child in level.transform)
        {
            if (child.tag == "Stage")
            {
                milestones.Add(child.gameObject.transform.Find("Start").gameObject);
            }
            if (child.tag == "End")
            {
                milestones.Add(child.gameObject);
            }
        }
        initCamera = new Vector3(camera.position.x, camera.position.y, camera.position.z);
        maxId = 0;
    }
    
    public void UpdateCamera()
    {
        int cameraIndex = GetCameraIndex();
        if (cameraIndex != -1)
        {
            PlaceCamera(milestones[cameraIndex].transform, milestones[cameraIndex + 1].transform);
        }
    }

    private void PlaceCamera(Transform left, Transform right)
    {
        float gapPos = (right.position.z + left.position.z) / 2f;
        float gap = right.position.z - left.position.z;
        float distance = (gap - 16f) / 2f;
        float height = distance * Mathf.Tan(Mathf.PI/6);

        camera.position = Vector3.SmoothDamp(camera.position,
                                                new Vector3(initCamera.x + distance, initCamera.y + height, gapPos),
                                                ref velocity, 
                                                smoothTime);
    }

    private float StartPlatformZ(Transform platform)
    {
        float zPos = platform.position.z;
        float zShift = platform.localScale.z / 2;
        return zPos - zShift;
    }

    public int GetCameraIndex()
    {
        int index1 = 0;
        int index2 = 0;

        for (int i = 0; i < milestones.Count - 1; i++)
        {
            if (player1.position.z > StartPlatformZ(milestones[i].transform) && player1.position.z < StartPlatformZ(milestones[i + 1].transform))
            {
                index1 = i;
            }
            if (player2.position.z > StartPlatformZ(milestones[i].transform) && player2.position.z < StartPlatformZ(milestones[i + 1].transform))
            {
                index2 = i;
            }
        }

        if (player1.position.z <= StartPlatformZ(milestones[0].transform))
        {
            index1 = 0;
        }
        if (player2.position.z <= StartPlatformZ(milestones[0].transform))
        {
            index2 = 0;
        }
        if (player1.position.z >= StartPlatformZ(milestones[milestones.Count - 2].transform))
        {
            index1 = milestones.Count - 2;
        }
        if (player2.position.z >= StartPlatformZ(milestones[milestones.Count - 2].transform))
        {
            index2 = milestones.Count - 2;
        }

        int index = Mathf.Min(index1, index2);

        if (index > maxId)
        {
            maxId = index;
        }

        if (index < maxId)
        {
            return maxId;
        }
        else
        {
            return index;
        }
    }
}
