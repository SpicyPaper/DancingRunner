using UnityEngine;

/// <summary>
/// Scale a platform on itself
/// </summary>
public class PlatformScaling : MonoBehaviour
{
    public float speed;
    public Vector3 scale;
    public Vector3 timeShift;
    
    private Vector3 initScale;

    // Start is called before the first frame update
    void Start()
    {
        initScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = initScale + new Vector3( scale.x * ((1 + Mathf.Sin(timeShift.x + Time.time * speed)) / 2), 
                                                        scale.y * ((1 + Mathf.Sin(timeShift.y + Time.time * speed)) / 2), 
                                                        scale.z * ((1 + Mathf.Sin(timeShift.z + Time.time * speed)) / 2) );
    }
}
