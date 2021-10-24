using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 _upAxis;

    #region Movement Settings
    [Header("Movement Settings")]

    [SerializeField, Range(3f, 200f)]
    public float _moveAccel = 8f;

    [SerializeField, Range(5f, 20f)]
    public float _rotationSpeed = 10f;

    [SerializeField, Range(0.1f, 200f)]
    public float _jumpVel = 10f;

    [SerializeField, Range(1f, 200f)]
    float _maxSpeed = 10f;
    [SerializeField, Range(1f, 200f)]
    float _maxSprintSpeed = 20f;

    [SerializeField, Range(0f, 5f)]
    float _airMoveMultiplier = 0.4f;

    [SerializeField, Range(0f, 1f)]
    float _groundDragThreshold = 0.1f;
    [SerializeField, Range(0f, 10f)]
    float _groundDrag = 5f;
    #endregion

    [Header("Ground Check")]
    [SerializeField]
    float _spherecastDist = 0.5f;
    [SerializeField]
    float _spherecastStartOffset = 0.5f;

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
        _isGrounded = CheckGrounded();
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector3 forwardMoveDir = cameraObject.forward * inputManager._verticalInput;
        Vector3 lateralMoveDir = cameraObject.right * inputManager._horizontalInput;
        forwardMoveDir = ProjectDirectionOnPlane(forwardMoveDir, _upAxis);
        lateralMoveDir = ProjectDirectionOnPlane(lateralMoveDir, _upAxis);

        // If we're going too fast, don't add speed in that direction.
        float max = _isSprint ? _maxSprintSpeed : _maxSpeed;
        if (Vector3.Dot(forwardMoveDir, playerRigidbody.velocity) > max)
        {
            forwardMoveDir = Vector3.zero;
        }
        if (Vector3.Dot(lateralMoveDir, playerRigidbody.velocity) > max)
        {
            lateralMoveDir = Vector3.zero;
        }

        _moveDirection = forwardMoveDir + lateralMoveDir;
        if (_moveDirection.sqrMagnitude > 0f)
        {
            _moveDirection.Normalize();
        }

        Vector3 moveAccel = _moveDirection * _moveAccel;
        if (!_isGrounded)
        {
            moveAccel *= _airMoveMultiplier;
        }

        playerRigidbody.AddForce(moveAccel + _gravity);

        // Prevent sliding
        if (_isGrounded &&
            (_moveDirection.sqrMagnitude < _groundDragThreshold
            || Vector3.Dot(_moveDirection, playerRigidbody.velocity) < 0f))
        {
            playerRigidbody.drag = _groundDrag;
        }
        else
        {
            playerRigidbody.drag = 0f;
        }
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
        if (_isGrounded)
        {
            playerRigidbody.velocity += _upAxis * _jumpVel;
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jumping", false);
        }
    }

    private bool CheckGrounded()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position + _upAxis * _spherecastStartOffset, 0.4f, -_upAxis, out hit, _spherecastDist, _groundLayer);
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

    // private void OnCollisionEnter(Collision collision)
    // {
    //     EvaluateCollision(collision);
    // }

    // private void OnCollisionStay(Collision collision)
    // {
    //     EvaluateCollision(collision);
    // }

    // private void EvaluateCollision(Collision collision)
    // {
    //     for (int i = 0; i < collision.contactCount; i++)
    //     {
    //         Vector3 normal = collision.GetContact(i).normal;
    //         _isGrounded |= normal.y >= 0.9f;
    //         Debug.Log($"_isGrounded: {_isGrounded}");
    //     }

    // }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + _upAxis * _spherecastStartOffset, -_upAxis * _spherecastDist);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, _moveDirection);
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
