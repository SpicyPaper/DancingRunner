using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 7f;
    public float JumpHeight = 3f;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;
    public Transform PlayerCenter;

    private Rigidbody body;
    private Vector3 inputs = Vector3.zero;
    private bool isGrounded = true;
    private Vector3 movement;
    private Vector3 sumNormals;

    [SerializeField]
    private Transform groundChecker;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        inputs = Vector3.zero;
        inputs.x = Input.GetAxis("Horizontal");
        inputs.z = Input.GetAxis("Vertical");
        if (inputs != Vector3.zero)
            transform.forward = inputs;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
        
    }

    void FixedUpdate()
    {
        movement = inputs * Speed * Time.fixedDeltaTime;

        //// Get the velocity
        //Vector3 horizontalMove = movement;


        //// Don't use the vertical velocity
        //horizontalMove.y = 0;
        //// Calculate the approximate distance that will be traversed
        //float distance = horizontalMove.magnitude;
        //// Normalize horizontalMove since it should be used to indicate direction
        //horizontalMove.Normalize();
        //RaycastHit hit;
        //// Check if the body's current velocity will result in a collision
        ////if (body.SweepTest(horizontalMove, out hit, distance))
        //RaycastHit[] hits = Physics.RaycastAll(PlayerCenter.position, horizontalMove, distance);
        //if (hits.Length > 0)
        //{

        //    movement -= Vector3.Project(movement, -hits[0].normal);

        //    // If so, stop the movement
        //    //inputs += -hits[0].normal * Speed;
        //}
        
        //Vector3 cancelForce = Vector3.Project(movement, -sumNormals);
        //if (cancelForce != movement)
        //    movement -= Vector3.Project(movement, -sumNormals);

        body.MovePosition(body.position + movement);
    }
    
    private void OnCollisionStay(Collision collision)
    {
        sumNormals = new Vector3();
        foreach (ContactPoint contactPoint in collision.contacts)
        {
            sumNormals += contactPoint.normal;
        }
    }
}
