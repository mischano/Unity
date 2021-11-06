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
    public float moveAccel = 50f;

    [SerializeField, Range(1f, 10000f)]
    public float rotationSpeed = 500f;

    [SerializeField, Range(0.1f, 200f)]
    public float jumpVel = 10f;

    [SerializeField, Range(1f, 200f)]
    float _maxSpeed = 5f;
    [SerializeField, Range(1f, 200f)]
    float _maxSprintSpeed = 10f;

    [SerializeField, Range(0f, 5f)]
    float _airMoveMultiplier = 0.4f;
    [SerializeField, Range(0f, 5f)]
    float _zeroGMoveMultiplier = 0.4f;
    [SerializeField, Range(0f, 200f)]
    float _jumpHoldVel = 13f;
    [SerializeField]
    float _zeroGMoveOxygenDepleteRate = 10f;

    [SerializeField, Range(0f, 1f)]
    float _groundDragThreshold = 0.1f;
    [SerializeField, Range(0f, 10f)]
    float _groundDragStopping = 5f;
    [SerializeField, Range(0f, 2f)]
    float _groundDragMoving = 1f;
    [SerializeField, Range(0f, 2f)]
    float _airDrag = 1f;

    [SerializeField]
    float _groundDownForceMultiplier = 2f;

    [SerializeField]
    int _numJumpingTicks = 30;
    #endregion

    [Header("Ground Check")]
    [SerializeField]
    float _spherecastDist = 1f;
    [SerializeField]
    float _spherecastStartOffset = 1f;

    #region Movement Flags
    [Header("Animation Flags")]
    public bool isSprint;
    public bool isJumping;
    public bool isGrounded;
    public bool isFalling => !isGrounded && !isJumping;
    public bool isWalking => _isMoving && !isSprint;
    bool _isMoving;
    #endregion

    [Header("Falling Settings")]
    public float inAirTime;
    public LayerMask playerLayer;
    private LayerMask _playerLayerMask;

    private Transform _cameraObject;
    private InputManager _inputManager;
    private PlayerManager _playerManager;
    [Header("Visual")]
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

    #region Internal Flags
    bool _inZeroGravity;
    #endregion
    public bool isDead;

    private Vector3 _moveDirection;
    private Vector3 _forwardMoveDir;
    private Vector3 _lateralMoveDir;
    private Vector3 _gravity;

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _playerManager = GetComponent<PlayerManager>();

        _rb = GetComponent<Rigidbody>();
        _oxygen = GetComponent<Oxygen>();
        _cameraObject = Camera.main.transform;

        _playerLayerMask = ~playerLayer;

        isJumping = false;
        isDead = false;
        _isMoving = false;
    }

    public void HandleAllMovement()
    {
        _gravity = GetGravity();
        isGrounded = CheckGrounded();
        GetMoveDirection();
        HandleMovement();
        if (_inZeroGravity)
        {
            HandleZeroGRotation();
        }
        else
        {
            HandleRotation();
        }
        HandleDrag();
        if (isGrounded)
        {
            _oxygen.RefillOxygen();
        }
        // _visualObject gets interpolated thanks to InterpolatedTransform
        _visualObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    void GetMoveDirection()
    {
        _forwardMoveDir = _cameraObject.forward * _inputManager._verticalInput;
        _lateralMoveDir = _cameraObject.right * _inputManager._horizontalInput;
        _isMoving = _forwardMoveDir != Vector3.zero || _lateralMoveDir != Vector3.zero;

        if (!_inZeroGravity)
        {
            _forwardMoveDir = Vector3.ProjectOnPlane(_forwardMoveDir, _upAxis);
            _lateralMoveDir = Vector3.ProjectOnPlane(_lateralMoveDir, _upAxis);
        }

        _moveDirection = _forwardMoveDir + _lateralMoveDir;
        _moveDirection = Vector3.ClampMagnitude(_moveDirection, 1.0f);
    }

    private void HandleMovement()
    {
        if (isDead)
        {
            return;
        }

        Vector3 accel = _moveDirection;
        // If we're going too fast, don't add speed in that direction.
        float max = isSprint ? _maxSprintSpeed : _maxSpeed;
        if (Vector3.Dot(_forwardMoveDir, _rb.velocity) > max)
        {
            accel -= _forwardMoveDir;
        }
        if (Vector3.Dot(_lateralMoveDir, _rb.velocity) > max)
        {
            accel -= _lateralMoveDir;
        }
        accel *= moveAccel;

        if (!isGrounded && accel != Vector3.zero)
        {
            // Handle air/zeroG movement
            if (_inZeroGravity)
            {
                _oxygen.TakeDamage(_zeroGMoveOxygenDepleteRate * Time.deltaTime);
                accel *= _zeroGMoveMultiplier;
            }
            else
            {
                accel *= _airMoveMultiplier;
            }
        }
        else if (isGrounded && !isJumping)
        {
            // Also apply a downward force proportional to velocity
            _rb.AddForce(-_upAxis * _groundDownForceMultiplier * _rb.velocity.magnitude);
        }

        _rb.AddForce(accel);
    }

    void HandleDrag()
    {
        // Change drag based on movement state
        if (_inZeroGravity)
        {
            _rb.drag = 0f;
        }
        else if (!isGrounded)
        {
            _rb.drag = _airDrag;
        }
        else if (_moveDirection.sqrMagnitude < _groundDragThreshold
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
        Vector3 targetDirection = _moveDirection;
        if (targetDirection == Vector3.zero || isDead)
        {
            targetDirection = transform.forward;
        }
        targetDirection = Vector3.ProjectOnPlane(targetDirection, _upAxis);
        targetDirection.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, _upAxis);
        Quaternion newRotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        // This doesn't interpolate between FixedUpdates because _rb is not kinematic
        _rb.MoveRotation(newRotation);
    }

    public void HandleJumping()
    {
        if (!isGrounded || isJumping)
        {
            return;
        }
        Vector3 upVelocity = Vector3.Project(_rb.velocity, _upAxis);
        Vector3 desiredUpVelocity = _upAxis * jumpVel;
        _rb.AddForce(desiredUpVelocity - upVelocity, ForceMode.VelocityChange);
        StopCoroutine(JumpingCoroutine());
        StartCoroutine(JumpingCoroutine());
    }

    IEnumerator JumpingCoroutine()
    {
        float velPerTick = _jumpHoldVel / (float)_numJumpingTicks;
        isJumping = true;
        for (int i = 0; i < _numJumpingTicks; i++)
        {
            if (!Input.GetButton("Jump"))
            {
                isJumping = false;
                yield break;
            }
            // Still holding jump
            _rb.AddForce(_upAxis * velPerTick, ForceMode.VelocityChange);
            yield return new WaitForFixedUpdate();
        }
        isJumping = false;
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
        Quaternion targetUpRotation = Quaternion.FromToRotation(transform.up, _cameraObject.up) * transform.rotation;
        Quaternion newUpRotation = Quaternion.Slerp(transform.rotation, targetUpRotation, rotateAmount);

        _upAxis = newUpRotation * transform.up;
        _rb.MoveRotation(newUpRotation);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _upAxis);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, _moveDirection);
    }
}
