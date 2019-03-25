using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);

        if (Input.GetKeyDown("space") && IsGrounded())
        {
            rb.AddForce(Vector3.up * 400);
        }
    }

    private bool IsGrounded()
    {
        Collider collider = GetComponent<Collider>();
        float rayCastSize = 0.1f;

        return Physics.Raycast(
            collider.bounds.center,
            Vector3.down,
            out RaycastHit rayHit,
            collider.bounds.extents.y + rayCastSize);
    }

}
