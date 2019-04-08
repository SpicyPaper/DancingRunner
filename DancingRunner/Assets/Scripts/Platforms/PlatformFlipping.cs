using UnityEngine;

public class PlatformFlipping : MonoBehaviour
{
    public float pauseDuration;
    public float speed;
    public Vector3 axis;

    private float timePassed;
    private float angleAccumulated;
    private bool turning;

    private const float SPEED_FACTOR = 100;

    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0f;
        angleAccumulated = 0f;
        turning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (turning)
        {
            float angle = Time.deltaTime * speed * SPEED_FACTOR;
            float lastAngleAccumulated = angleAccumulated;
            angleAccumulated += angle;

            if (angleAccumulated > 90)
            {
                angle = 90 - lastAngleAccumulated;
                angleAccumulated = 0;
                turning = false;
                timePassed = 0;
            }

            transform.Rotate(axis * angle, Space.Self);
        }
        else
        {
            timePassed += Time.deltaTime;

            if (timePassed > pauseDuration)
            {
                timePassed = 0;
                turning = true;
            }
        }
    }
}
