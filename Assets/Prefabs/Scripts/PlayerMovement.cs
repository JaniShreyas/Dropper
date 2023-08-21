using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    public float walkSpeed = 10f;
    public float sprintSpeed = 17.5f;
    public float sprintForce = 80f;
    public float sprintCooldown = 2f;

    private float sprintTimer = Mathf.Infinity;

    public float multiplier = 1f;
    public float slopeMultiplier = 2f;
    public float airMultiplier = 0.4f; //Keep between 0 & 1

    [NonSerialized]
    public float moveSpeed = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    public float groundDistance = 0.4f;

    public LayerMask groundMask;
    
    public Transform groundCheck;


    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    
    float vertical;
    float horizontal;

    float groundDrag = 6f;
    float airDrag = 2f;

    bool isGrounded;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rigidBody;

    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;

        moveSpeed = walkSpeed;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        PlayerInput();
        ControlDrag();
        JumpInput();
        SprintInput();

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        sprintTimer += Time.deltaTime;
    }

    private void ControlDrag()
    {
        if (isGrounded)
        {
            rigidBody.drag = groundDrag;
        }
        else
        {
            rigidBody.drag = airDrag;
        }
    }

    private void PlayerInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * vertical + orientation.right * horizontal;
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void SprintInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //if(sprintTimer >= sprintCooldown)
            //{
            //    rigidBody.AddForce(orientation.forward * sprintForce, ForceMode.Impulse);
            //    sprintTimer = 0f;
            //}
            moveSpeed = sprintSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
        }
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rigidBody.AddForce(moveDirection.normalized * moveSpeed * multiplier, ForceMode.Acceleration);
        }
        else if(isGrounded && OnSlope())
        {
            rigidBody.AddForce(slopeMoveDirection.normalized * moveSpeed * multiplier * slopeMultiplier, ForceMode.Acceleration);
        }
        else if(!isGrounded)
        {
            rigidBody.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * multiplier/Mathf.Abs(multiplier), ForceMode.Acceleration);
        }
    }
}
