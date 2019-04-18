using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int PlayerId = 1;
    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float WallJumpPropulsion = 2f;
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
    private bool canControlCharacter = true;
    private Vector3 warpPosition = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAnimator = GetComponentInChildren<Animator>();
        colorChanger = GetComponent<ColorChanger>();
    }

    void Update()
    {
        // Color change
        if (Input.GetButtonDown("ChangeColorP" + PlayerId))
            colorChanger.ChangeColor();
        
        isGrounded = controller.isGrounded;

        // Movement
        if (isGrounded && velocity.y < 0)
        {
            velocity = Vector3.zero;
            playerAnimator.SetBool("IsJumping", false);
        }

        if (canControlCharacter)
        {
            Vector3 move = new Vector3(Input.GetAxisRaw("HorizontalP" + PlayerId), 0, Input.GetAxisRaw("VerticalP" + PlayerId)).normalized;

            move = Quaternion.LookRotation(Camera.main.transform.forward) * move;
            move.y = 0;
            controller.Move(move * Time.deltaTime * Speed);

            if (move != Vector3.zero)
                Move(move);
            else
                StopMove();
        }
        
        // Jump / wall jump
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

    void LateUpdate()
    {
        // Used to teleport correctly the player
        if (warpPosition != Vector3.zero)
        {
            transform.position = warpPosition;
            warpPosition = Vector3.zero;
        }
    }

    /// <summary>
    /// Teleport the player to the specified position
    /// </summary>
    /// <param name="newPosition">New position of the player</param>
    public void WarpToPosition(Vector3 newPosition)
    {
        warpPosition = newPosition;
    }

    /// <summary>
    /// Execute a wall jump
    /// </summary>
    private void WallJump()
    {
        velocity = wallNormal.normalized * Mathf.Sqrt(WallJumpPropulsion * -2f * Gravity);
        velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        canControlCharacter = false;
        StartCoroutine(RotatePlayer(transform.rotation, 0.7f));
    }

    /// <summary>
    /// Rotate the player by 180 degrees
    /// </summary>
    /// <param name="startRotation">The rotation of the player at the start of the call</param>
    /// <param name="duration">The duration that the rotation will take to complete</param>
    /// <returns></returns>
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
        canControlCharacter = true;

    }

    /// <summary>
    /// Check whether the player is on a wall (with a raycast) and set the animation accordingly
    /// </summary>
    /// <returns>True if the player is on the wall, false otherwise</returns>
    private bool IsOnWall()
    {
        Vector3 sumNormals = new Vector3();
        foreach (RaycastHit hit in Physics.RaycastAll(PlayerFront.position, transform.forward, 0.1f))
        {
            sumNormals += hit.normal;
        }
        if ((int)sumNormals.y == 0 && (sumNormals.x != 0 || sumNormals.z != 0) && !isGrounded)
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
    
    /// <summary>
    /// Stop the player animation
    /// </summary>
    private void StopMove()
    {
        playerAnimator.SetBool("IsRunning", false);
    }

    /// <summary>
    /// Move the player and set the running animation
    /// </summary>
    /// <param name="move"></param>
    private void Move(Vector3 move)
    {
        transform.forward = move;
        playerAnimator.SetBool("IsRunning", true);
    }

    /// <summary>
    /// Make the player jump and set the animation accordingly
    /// </summary>
    private void Jump()
    {
        velocity.y += Mathf.Sqrt(JumpHeight * -2f * Gravity);
        playerAnimator.SetBool("IsJumping", true);
    }
}
