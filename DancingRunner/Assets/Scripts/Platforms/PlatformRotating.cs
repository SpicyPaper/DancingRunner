using UnityEngine;

/// <summary>
/// Rotate a platform on itself
/// </summary>
public class PlatformRotating : MonoBehaviour
{
    public float speed;

    private const float SPEED_CONST = 50;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * SPEED_CONST * Time.deltaTime * speed, Space.Self);
    }
}
