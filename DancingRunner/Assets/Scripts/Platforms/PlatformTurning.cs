using UnityEngine;

/// <summary>
/// Turn a platform in circle around it's center or only on a single axis
/// </summary>
public class PlatformTurning : MonoBehaviour
{
    public float speed;
    public float radiusX;
    public float shiftX;
    public float radiusZ;
    public float shiftZ;

    private Vector3 initPos;

    // Start is called before the first frame update
    void Start()
    {
        initPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initPos + new Vector3(radiusX * Mathf.Sin(Time.time * speed + shiftX), 0f, radiusZ * Mathf.Cos(Time.time * speed + shiftZ));
    }
}
