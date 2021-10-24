using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 _upAxis;

    #region Movement Settings
    [Header("Movement Settings")]

    [SerializeField, Range(1f, 15f)]
    public float _walkSpeed = 5f;

    [SerializeField, Range(3f, 30f)]
    public float _sprintSpeed = 8f;

    [SerializeField, Range(5f, 20f)]
    public float _rotationSpeed = 10f;

    [SerializeField, Range(0.1f, 200f)]
    public float _jumpVel = 10f;
    #endregion

    #region Movement Flags
    [Header("Movement Flags")]
    public bool _isSprint;
    public bool _isJumping;
    public bool _isGrounded;
    #endregion

    [Header("Falling Settings")]
    public float _inAirTime;
    public LayerMask _groundLayer;

    private Transform cameraObject;
    private InputManager inputManager;
    private PlayerManager playerManager;
    private AnimatorManager animatorManager;

    private Rigidbody playerRigidbody;

    private Vector3 _moveDirection;
    private Vector3 _gravity;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();

        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }


    public void HandleAllMovement()
    {
        _gravity = GetGravity();
        HandleMovement();
        HandleRotation();
        // HandleJumping();
    }

    private void HandleMovement()
    {
        _moveDirection = cameraObject.forward * inputManager._verticalInput;
        _moveDirection += cameraObject.right * inputManager._horizontalInput;
        _moveDirection.Normalize();

        float speed = _isSprint ? _sprintSpeed : _walkSpeed;
        _moveDirection = ProjectDirectionOnPlane(_moveDirection, _upAxis);
        _moveDirection *= speed;


        Vector3 movementVelocity = _moveDirection;

        movementVelocity += _gravity;

        playerRigidbody.velocity += movementVelocity * Time.deltaTime;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection;

        targetDirection = cameraObject.forward * inputManager._verticalInput;
        targetDirection += cameraObject.right * inputManager._horizontalInput;
        targetDirection.Normalize();

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        targetDirection = ProjectDirectionOnPlane(targetDirection, _upAxis);

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    public void HandleJumping()
    {
        _isGrounded = true;
        if (_isGrounded)
        {
            Debug.Log("jumping");
            playerRigidbody.velocity += _upAxis * _jumpVel;
            // animatorManager.animator.SetBool("isJumping", true);
            // animatorManager.PlayTargetAnimation("Jumping", false);
        }
    }

    private Vector3 GetGravity()
    {
        Vector3 gravity = CustomGravity.GetGravity(playerRigidbody.position, out _upAxis);
        return gravity;
    }
    private Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            _isGrounded |= normal.y >= 0.9f;
        }

    }

    void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position, _upAxis * _jumpVel);
    }

    //private void HandleFallingAndLanding()
    //{
    //    RaycastHit hit;
    //    Vector3 rayCastOrigin = transform.position;

    //    if (!_isGrounded)
    //    {
    //        if (!playerManager._isInteracting)
    //        {
    //            animatorManager.PlayTargetAnimation("Falling", true);
    //        }

    //        // _inAirTime += Time.deltaTime;
    //        playerRigidbody.AddForce(GetGravity());
    //    }
    //    Debug.Log((Physics.Raycast(playerRigidbody.position, -upAxis, out hit, 0.2f, _groundLayer)));

    //    if (Physics.Raycast(playerRigidbody.position, -upAxis, out hit, 0.2f, _groundLayer))
    //    {
    //        if (!_isGrounded && playerManager._isInteracting)
    //        {
    //            animatorManager.PlayTargetAnimation("Landing", true);
    //        }
    //        _isGrounded = true;
    //    }
    //    else
    //    {
    //        _isGrounded = false;
    //    }
    //}

}
