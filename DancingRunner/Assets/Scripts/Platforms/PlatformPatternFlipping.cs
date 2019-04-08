using UnityEngine;

public class PlatformPatternFlipping : MonoBehaviour
{
    public float pauseDuration;
    public float speed;
    public Vector3[] pattern;

    private float timePassed;
    private float angleAccumulated;
    private bool turning;
    private int indexPattern;
    private Vector3 selectedAxis;

    private const float SPEED_FACTOR = 100;

    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0f;
        angleAccumulated = 0f;
        indexPattern = 0;
        turning = true;

        if (pattern.Length > 0)
        {
            selectedAxis = pattern[indexPattern];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pattern.Length > 0)
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

                transform.Rotate(selectedAxis * angle, Space.Self);
            }
            else
            {
                timePassed += Time.deltaTime;

                if (timePassed > pauseDuration)
                {
                    timePassed = 0;
                    turning = true;

                    indexPattern++;
                    if (indexPattern >= pattern.Length)
                    {
                        indexPattern = 0;
                    }
                    selectedAxis = pattern[indexPattern];
                }
            }
        }
    }
}
