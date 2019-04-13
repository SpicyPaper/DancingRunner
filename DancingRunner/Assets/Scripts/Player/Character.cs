using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int PlayerId = 1;
    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float Gravity = -9.81f;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;
    public Vector3 Drag;

    [SerializeField]
    private Transform GroundChecker;
    [SerializeField]
    private Transform PlayerFront;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded = true;
    private Animator playerAnimator;
    private Vector3 wallNormal;
    private ColorChanger colorChanger;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAnimator = GetComponentInChildren<Animator>();
        colorChanger = GetComponent<ColorChanger>();
    }

    void Update()
    {
        if (Input.GetButtonDown("ChangeColorP" + PlayerId))
            colorChanger.ChangeColor();

        isGrounded = Physics.CheckSphere(GroundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);
        if (isGrounded && velocity.y < 0)
        {
            velocity = Vector3.zero;
            playerAnimator.SetBool("IsJumping", false);
        }

        Vector3 move = new Vector3(Input.GetAxis("HorizontalP" + PlayerId), 0, Input.GetAxis("VerticalP" + PlayerId)).normalized;

        move = Quaternion.LookRotation(Camera.main.transform.forward) * move;
        move.y = 0;
        controller.Move(move * Time.deltaTime * Speed);
        
        if (move != Vector3.zero)
            Move(move);
        else
            StopMove();

        bool isOnWall = IsOnWall();

        if (Input.GetButtonDown("JumpP" + PlayerId))
            if (isGrounded)
                Jump();
            else if (isOnWall)
                WallJump();

        velocity.y += Gravity * Time.deltaTime;

        velocity.x /= 1 + Drag.x * Time.deltaTime;
        velocity.y /= 1 + Drag.y * Time.deltaTime;
        velocity.z /= 1 + Drag.z * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void WallJump()
    {
        velocity = wallNormal * Mathf.Sqrt(JumpHeight * -2f * Gravity);
        velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        StartCoroutine(RotatePlayer(transform.rotation, 1.2f));
    }



    IEnumerator RotatePlayer(Quaternion startRotation, float duration)
    {
        Vector3 rot = startRotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y + 180, rot.z);
        Quaternion endRotation = Quaternion.Euler(rot);


        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= duration)
        { // until one second passed
            transform.rotation = Quaternion.Lerp(transform.rotation, endRotation, Time.time - startTime);
            yield return 1; // wait for next frame
        }
        
    }

    private bool IsOnWall()
    {
        Vector3 sumNormals = new Vector3();
        foreach (RaycastHit hit in Physics.RaycastAll(PlayerFront.position, transform.forward, 0.1f))
        {
            sumNormals += hit.normal;
        }
        if (sumNormals.y == 0 && (sumNormals.x != 0 || sumNormals.z != 0) && !isGrounded)
        {
            wallNormal = sumNormals;
            playerAnimator.SetBool("IsOnWall", true);
            return true;
        }
        else
            wallNormal = Vector3.zero;
        
        playerAnimator.SetBool("IsOnWall", false);
        return false;
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
