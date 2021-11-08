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
    public bool isJumping; // TODO unify this with private _movementJumping
    public bool isGrounded;
    #endregion

    [Header("Falling Settings")]
    public float inAirTime;
    public LayerMask playerLayer;
    private LayerMask _playerLayerMask;

    private Transform _cameraObject;
    private InputManager _inputManager;
    private PlayerManager _playerManager;
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

    #region Internal Flags
    bool _inZeroGravity;
    bool _movementJumping;
    #endregion
    public bool isDead;

    private Vector3 _moveDirection;
    private Vector3 _gravity;

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _playerManager = GetComponent<PlayerManager>();
        _animatorManager = _visualObject.GetComponent<AnimatorManager>();

        _rb = GetComponent<Rigidbody>();
        _oxygen = GetComponent<Oxygen>();
        _cameraObject = Camera.main.transform;

        _playerLayerMask = ~playerLayer;

        _movementJumping = false;
        isDead = false;
    }

    public void HandleAllMovement()
    {
        _gravity = GetGravity();
        isGrounded = CheckGrounded();
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

    private void HandleMovement()
    {
        if (isDead)
        {
            return;
        }
        Vector3 forwardMoveDir = _cameraObject.forward * _inputManager._verticalInput;
        Vector3 lateralMoveDir = _cameraObject.right * _inputManager._horizontalInput;

        if (!_inZeroGravity)
        {
            forwardMoveDir = Vector3.ProjectOnPlane(forwardMoveDir, _upAxis);
            lateralMoveDir = Vector3.ProjectOnPlane(lateralMoveDir, _upAxis);
        }

        // If we're going too fast, don't add speed in that direction.
        float max = isSprint ? _maxSprintSpeed : _maxSpeed;
        if (Vector3.Dot(forwardMoveDir, _rb.velocity) > max)
        {
            forwardMoveDir = Vector3.zero;
        }
        if (Vector3.Dot(lateralMoveDir, _rb.velocity) > max)
        {
            lateralMoveDir = Vector3.zero;
        }

        _moveDirection = forwardMoveDir + lateralMoveDir;
        _moveDirection = Vector3.ClampMagnitude(_moveDirection, 1.0f);

        Vector3 accel = _moveDirection * moveAccel;
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
        else if (isGrounded && !_movementJumping)
        {
            // Also apply a downward force proportional to velocity
            _rb.AddForce(-_upAxis * _groundDownForceMultiplier * _rb.velocity.magnitude);
        }

        _rb.AddForce(accel + _gravity);
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
        // targetDirection = _cameraObject.forward * _inputManager._verticalInput
        //     + _cameraObject.right * _inputManager._horizontalInput;
        if (targetDirection.sqrMagnitude < 0.05f || isDead)
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
        if (!isGrounded || _movementJumping)
        {
            return;
        }
        Vector3 upVelocity = Vector3.Project(_rb.velocity, _upAxis);
        Vector3 desiredUpVelocity = _upAxis * jumpVel;
        // _rb.velocity += desiredUpVelocity - upVelocity;
        _rb.AddForce(desiredUpVelocity - upVelocity, ForceMode.VelocityChange);
        _animatorManager.animator.SetBool("isJumping", true);
        _animatorManager.PlayTargetAnimation("Jumping", false);
        StopCoroutine(JumpingCoroutine());
        StartCoroutine(JumpingCoroutine());
    }

    IEnumerator JumpingCoroutine()
    {
        float velPerTick = _jumpHoldVel / (float)_numJumpingTicks;
        _movementJumping = true;
        for (int i = 0; i < _numJumpingTicks; i++)
        {
            if (!Input.GetButton("Jump"))
            {
                _movementJumping = false;
                yield break;
            }
            // Still holding jump
            _rb.AddForce(_upAxis * velPerTick, ForceMode.VelocityChange);
            yield return new WaitForFixedUpdate();
        }
        _movementJumping = false;
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

    // void JumpAdjustGravity()
    // {
    //     // If we aren't falling (have upwards velocity)
    //     if (!_inZeroGravity && Vector3.Dot(_rb.velocity, _upAxis) > 0f)
    //     {
    //         // Reduce gravity by adding force opposite to gravity
    //         _rb.AddForce(-_gravity * (1 - _holdingJumpGravityMultiplier), ForceMode.Acceleration);
    //     }
    // }

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
