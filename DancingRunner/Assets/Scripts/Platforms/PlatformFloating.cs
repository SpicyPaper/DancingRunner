using UnityEngine;

public class PlatformFloating : MonoBehaviour
{
    public float speed;
    public float height;

    private Vector3 initPos;
    
    // Start is called before the first frame update
    void Start()
    {
        initPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initPos + new Vector3(0f, height * Mathf.Sin(Time.time * speed), 0f);
    }
}
