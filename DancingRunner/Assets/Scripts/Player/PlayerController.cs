using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is a test for the player controller with a rigidbody.
/// See the Character class that user a character controller instead.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float Speed = 7f;
    public float JumpHeight = 3f;
    public float GroundDistance = 0.2f;
    public float WallJumpForce = 600;
    public LayerMask Ground;

    private Rigidbody body;
    private Vector3 inputs = Vector3.zero;
    private bool isGrounded = true;
    private bool isOnWall = false;
    private Vector3 movement;
    private Vector3 sumNormals;
    private Animator playerAnimator;

    [SerializeField]
    private Transform groundChecker;

    void Start()
    {
        // Load components once instead of doing it in the update loop
        body = GetComponent<Rigidbody>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        inputs = Vector3.zero;
        inputs.x = Input.GetAxis("Horizontal");
        inputs.z = Input.GetAxis("Vertical");
        if (inputs != Vector3.zero)
            transform.forward = inputs;

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
                playerAnimator.SetBool("IsJumping", true);
            }
            if (isOnWall)
            {
                body.AddForce(Vector3.Normalize(Vector3.up + Vector3.Normalize(sumNormals)) * WallJumpForce, ForceMode.Impulse);
                isOnWall = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (isGrounded)
            body.velocity = new Vector3(0, body.velocity.y, 0);

        movement = inputs * Speed * Time.fixedDeltaTime;
        
        playerAnimator.SetBool("IsRunning", movement != Vector3.zero);

        body.MovePosition(body.position + movement);
    }
    
    private void OnCollisionStay(Collision collision)
    {
        sumNormals = new Vector3();
        foreach (ContactPoint contactPoint in collision.contacts)
        {
            sumNormals += contactPoint.normal;
        }

        isOnWall = sumNormals.y == 0;
    }

    private void OnCollisionExit(Collision collision)
    {
        isOnWall = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGrounded)
            playerAnimator.SetBool("IsJumping", false);
    }
}
