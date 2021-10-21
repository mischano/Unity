using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 upAxis;



    #region Movement Settings
    [Header("Movement Settings")]

    [SerializeField, Range(1f, 15f)]
    public float _walkSpeed = 5f;

    [SerializeField, Range(3f, 30f)]
    public float _sprintSpeed = 8f;

    [SerializeField, Range(5f, 20f)]
    public float _rotationSpeed = 10f;
    #endregion
   
    #region Movement Flags
    [Header("Movement Flags")]
    public bool _isSprint;
    public bool _isJumping;
    public bool _isGrounded;
    #endregion

    private Transform cameraObject;
    private InputManager inputManager;
    private Rigidbody playerRigidbody;

    private Vector3 moveDirection;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }
    
    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
        GetGravity();
    }
    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager._verticalInput;
        moveDirection += cameraObject.right * inputManager._horizontalInput;
        moveDirection.Normalize();
        // moveDirection.y = 0f;

        float speed = _isSprint ? _sprintSpeed : _walkSpeed;
        moveDirection = ProjectDirectionOnPlane(moveDirection, upAxis);
        moveDirection *= speed;


        Vector3 movementVelocity = moveDirection;

        Vector3 gravity = GetGravity();
        movementVelocity += gravity;
        playerRigidbody.velocity = movementVelocity;

        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection;

        targetDirection = cameraObject.forward * inputManager._verticalInput;
        targetDirection += cameraObject.right * inputManager._horizontalInput;
        targetDirection.Normalize();
        // targetDirection.y = 0f;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        targetDirection = ProjectDirectionOnPlane(targetDirection, upAxis);

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private Vector3 GetGravity()
    {
        Vector3 gravity = CustomGravity.GetGravity(playerRigidbody.position, out upAxis);
        return gravity;
    }
    private Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }

}
