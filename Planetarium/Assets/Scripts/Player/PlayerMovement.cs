using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 _upAxis;

    #region Movement Settings
    [Header("Movement Settings")]

    [SerializeField, Range(3f, 200f)]
    public float _moveAccel = 8f;

    [SerializeField, Range(1f, 10000f)]
    public float _rotationSpeed = 500f;

    [SerializeField, Range(0.1f, 200f)]
    public float _jumpVel = 10f;

    [SerializeField, Range(1f, 200f)]
    float _maxSpeed = 10f;
    [SerializeField, Range(1f, 200f)]
    float _maxSprintSpeed = 20f;

    [SerializeField, Range(0f, 5f)]
    float _airMoveMultiplier = 0.1f;
    [SerializeField, Range(0f, 5f)]
    float _zeroGMoveMultiplier = 0.4f;
    [SerializeField]
    float _zeroGMoveOxygenDepleteRate = 1.0f;

    [SerializeField, Range(0f, 1f)]
    float _groundDragThreshold = 0.1f;
    [SerializeField, Range(0f, 10f)]
    float _groundDragStopping = 5f;
    [SerializeField, Range(0f, 2f)]
    float _groundDragMoving = 1f;
    [SerializeField, Range(0f, 2f)]
    float _airDrag = 0.5f;
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
    bool _inZeroGravity;
    #endregion

    [Header("Falling Settings")]
    public float _inAirTime;
    public LayerMask playerLayer;
    private LayerMask _playerLayerMask;

    private Transform cameraObject;
    private InputManager inputManager;
    private PlayerManager playerManager;
    [Header("Visual")]
    private AnimatorManager _animatorManager;
    [SerializeField]
    private GameObject _visualObject;

    [Header("Camera")]
    [SerializeField]
    CinemachineFreeLook _freelook;

    [Header("Zero G Rotation")]
    [SerializeField]
    float _zeroGRotationSpeed = 1f;

    private Rigidbody _rb;
    private Oxygen _oxygen;

    private Vector3 _moveDirection;
    private Vector3 _gravity;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        _animatorManager = _visualObject.GetComponent<AnimatorManager>();

        _rb = GetComponent<Rigidbody>();
        _oxygen = GetComponent<Oxygen>();
        cameraObject = Camera.main.transform;

        _playerLayerMask = ~playerLayer;
    }

    public void HandleAllMovement()
    {
        _gravity = GetGravity();
        _isGrounded = CheckGrounded();
        HandleMovement();
        if (_inZeroGravity)
        {
            HandleZeroGRotation();
        }
        else
        {
            HandleRotation();
        }
        if (_isGrounded)
        {
            _oxygen.RefillOxygen();
        }
        // _visualObject gets interpolated thanks to InterpolatedTransform
        _visualObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    private void HandleMovement()
    {
        Vector3 forwardMoveDir = cameraObject.forward * inputManager._verticalInput;
        Vector3 lateralMoveDir = cameraObject.right * inputManager._horizontalInput;

        if (!_inZeroGravity)
        {
            forwardMoveDir = ProjectDirectionOnPlane(forwardMoveDir, _upAxis);
            lateralMoveDir = ProjectDirectionOnPlane(lateralMoveDir, _upAxis);
        }

        // If we're going too fast, don't add speed in that direction.
        float max = _isSprint ? _maxSprintSpeed : _maxSpeed;
        if (_isGrounded && Vector3.Dot(forwardMoveDir, _rb.velocity) > max)
        {
            forwardMoveDir = Vector3.zero;
        }
        if (_isGrounded && Vector3.Dot(lateralMoveDir, _rb.velocity) > max)
        {
            lateralMoveDir = Vector3.zero;
        }

        _moveDirection = forwardMoveDir + lateralMoveDir;
        if (_moveDirection != Vector3.zero)
        {
            _moveDirection.Normalize();
        }

        Vector3 moveAccel = _moveDirection * _moveAccel;
        if (!_isGrounded && moveAccel != Vector3.zero)
        {
            // Handle air/zeroG movement
            if (_inZeroGravity)
            {
                _oxygen.TakeDamage(_zeroGMoveOxygenDepleteRate * Time.deltaTime);
                moveAccel *= _zeroGMoveMultiplier;
            }
            else
            {
                moveAccel *= _airMoveMultiplier;
            }
        }

        _rb.AddForce(moveAccel + _gravity);

        // Change drag based on movement state
        if (_inZeroGravity)
        {
            _rb.drag = 0f;
        }
        else if (!_isGrounded)
        {
            _rb.drag = _airDrag;
        }
        else
        if (_moveDirection.sqrMagnitude < _groundDragThreshold
            || Vector3.Dot(_moveDirection, _rb.velocity) < 0f)
        {
            _rb.drag = _groundDragStopping;
        }
        else
        {
            _rb.drag = _groundDragMoving;
        }
    }

    private void HandleRotation()
    {
        Vector3 targetDirection;
        targetDirection = cameraObject.forward * inputManager._verticalInput
            + cameraObject.right * inputManager._horizontalInput;
        if (targetDirection.sqrMagnitude < 0.05f)
        {
            targetDirection = transform.forward;
        }
        targetDirection = ProjectDirectionOnPlane(targetDirection, _upAxis);
        targetDirection.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, _upAxis);
        Quaternion newRotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        // This doesn't interpolate between FixedUpdates because _rb is not kinematic
        _rb.MoveRotation(newRotation);
    }

    public void HandleJumping()
    {
        if (_isGrounded)
        {
            Vector3 upVelocity = Vector3.Project(_rb.velocity, _upAxis);
            Vector3 desiredUpVelocity = _upAxis * _jumpVel;
            _rb.velocity += desiredUpVelocity - upVelocity;
            _animatorManager.animator.SetBool("isJumping", true);
            _animatorManager.PlayTargetAnimation("Jumping", false);
        }
    }

    private bool CheckGrounded()
    {
        if (_inZeroGravity)
        {
            return false;
        }
        RaycastHit hit;
        return Physics.SphereCast(transform.position + _upAxis * _spherecastStartOffset, 0.4f, -_upAxis, out hit, _spherecastDist, _playerLayerMask);
    }

    private Vector3 GetGravity()
    {
        Vector3 gravity = CustomGravity.GetGravity(_rb.position, out _upAxis);
        if (gravity == Vector3.zero)
        {
            _inZeroGravity = true;
        }
        else
        {
            _inZeroGravity = false;
        }
        return gravity;
    }

    private Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }

    void HandleZeroGRotation()
    {
        // Slowly move the player to the camera's up based on input.
        AxisState yAxis = _freelook.m_YAxis;
        float axisExtra = 0f;
        float axisEpsilon = 0.08f;
        // Note: input axis is inverted
        if (yAxis.Value >= yAxis.m_MaxValue - axisEpsilon)
        {
            axisExtra = Mathf.Abs(Mathf.Clamp(yAxis.m_InputAxisValue, Mathf.NegativeInfinity, 0f));
        }
        else if (yAxis.Value <= yAxis.m_MinValue + axisEpsilon)
        {
            axisExtra = Mathf.Clamp(yAxis.m_InputAxisValue, 0f, Mathf.Infinity);
        }

        float rotateAmount = axisExtra * _zeroGRotationSpeed;
        Quaternion targetUpRotation = Quaternion.FromToRotation(transform.up, cameraObject.up) * transform.rotation;
        Quaternion newUpRotation = Quaternion.Slerp(transform.rotation, targetUpRotation, rotateAmount);

        _upAxis = newUpRotation * transform.up;
        _rb.MoveRotation(newUpRotation);
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
        // Gizmos.DrawRay(transform.position + _upAxis * _spherecastStartOffset, -_upAxis * _spherecastDist);
        Gizmos.DrawRay(transform.position, _upAxis);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, _moveDirection);
        // Debug.Log($"{_freelook.m_YAxis.m_MinValue}, {_freelook.m_YAxis.m_MaxValue}, {_freelook.m_YAxis.Value}");
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
