using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float Gravity = -9.81f;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;
    public Vector3 Drag;

    [SerializeField]
    private Transform GroundChecker;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded = true;
    private Animator playerAnimator;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(GroundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
            playerAnimator.SetBool("IsJumping", false);
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * Speed);
        
        if (move != Vector3.zero)
            Move(move);
        else
            StopMove();

        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();


        velocity.y += Gravity * Time.deltaTime;

        velocity.x /= 1 + Drag.x * Time.deltaTime;
        velocity.y /= 1 + Drag.y * Time.deltaTime;
        velocity.z /= 1 + Drag.z * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void StopMove()
    {
        playerAnimator.SetBool("IsRunning", false);
    }

    private void Move(Vector3 move)
    {
        transform.forward = move;
        playerAnimator.SetBool("IsRunning", true);
    }

    private void Jump()
    {
        velocity.y += Mathf.Sqrt(JumpHeight * -2f * Gravity);
        playerAnimator.SetBool("IsJumping", true);
    }
}
