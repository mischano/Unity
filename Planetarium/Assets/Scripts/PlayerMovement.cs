using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;
    Transform cameraObject;
    Rigidbody playerRigidbody;
    
    Vector3 moveDirection;
    
    // MOVEMENT SPEED 
    [Header("Movement Speed")]
    [SerializeField, Range(5f, 15f)]
    float walkSpeed = 5f;
    [SerializeField, Range(15f, 30f)]
    float sprintSpeed = 8f;
    [SerializeField, Range(5f, 20f)]
    float rotationSpeed = 10f;
    // END MOVEMENT SPEED 

    [Header("Jump Speed")]
    public float jumpHeight = 10f;
    public float gravityIntensity = -15f;

    // MOVEMENT FLAGS
    [Header("Movement Flags")]
    public bool isSprint;
    public bool isJumping;
    public bool isGrounded;
    // END MOVEMENT FLAGS

    [Header("Falling")]
    [SerializeField, Range(1f, 5f)]
    float leapingVelocity = 2.5f;
    [SerializeField, Range(10f, 60f)]
    float fallingVelocity = 30f;
    float inAirTimer;
    public float rayCastHeightOffSet = 0.5f;
    public LayerMask groundLayer;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        if (playerManager.isInteracting)
        {
            return;
        }
        if (isJumping)
        {
            return;
        }
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0f;

        float speed = isSprint ? sprintSpeed : walkSpeed;
        moveDirection *= speed;
        
        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y += rayCastHeightOffSet;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer += Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if (!isGrounded && playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }
            inAirTimer = 0f;
            isGrounded = true;
        }
        
        else
        {
            isGrounded = false;
        }
    }
    
    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity;
        }
    }

    private void HandleRotation()
    {
        if (isJumping)
        {
            return;
        }

        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection += cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0f;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }
}
