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

    [SerializeField]
    public float moveAccel = 2500f;

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
    float _dashVel = 20f;

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
    public bool isWalking => isMoving && !isSprint;
    public bool isMoving;
    #endregion

    [Header("Falling Settings")]
    public float inAirTime;
    public LayerMask playerLayer;
    private LayerMask _playerLayerMask;

    // Component references
    private Transform _cameraObject;
    private InputManager _inputManager;
    private PlayerManager _playerManager;
    private Oxygen _oxygen;
    private PlayerEffects _playerEffects;
    private Rigidbody _rb;
    private CustomGravityRigidbody _cgrb;

    [Header("Visual")]
    [SerializeField]
    private GameObject _visualObject;
    [SerializeField]
    ParticleSystem _zeroGMoveParticle;
    float _originalRateOverDistance;
    [SerializeField]
    ParticleSystem _dashParticle;
    float _dashParticleRate;

    [Header("Camera")]
    [SerializeField]
    CinemachineFreeLook _freelook;

    [Header("Zero G Rotation")]
    [SerializeField]
    float _zeroGRotationSpeed = 1f;

    [Header("Oxygen")]
    [SerializeField]
    float _zeroGOxygenDrain = 1f;
    [SerializeField]
    float _zeroGMoveOxygenDrain = 10f;
    [SerializeField]
    float _groundOxygenReplenishRate = 100f;
    [SerializeField]
    float _airJumpOxygenCost = 70f;
    [SerializeField]
    float _dashOxygenCost = 70f;

    [Header("Audio")]
    AudioSource _audioSource;
    [SerializeField] AudioClip _dashAudio;
    bool _playingZeroGMoveEffects;

    #region Internal Flags
    bool _inZeroGravity;
    bool _zeroGMoving;
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
        _cgrb = GetComponent<CustomGravityRigidbody>();
        _oxygen = GetComponent<Oxygen>();
        _playerEffects = GetComponent<PlayerEffects>();
        _audioSource = GetComponent<AudioSource>();

        _cameraObject = Camera.main.transform;

        _playerLayerMask = ~playerLayer;

        isJumping = false;
        isDead = false;
        isMoving = false;
        // We can change this in other scripts e.g. based on shooting state
        isSprint = true;

        _playingZeroGMoveEffects = false;
        _zeroGMoving = false;

        _originalRateOverDistance = _zeroGMoveParticle.emission.rateOverDistance.constant;
        var dashParticleEmission = _dashParticle.emission;
        _dashParticleRate = dashParticleEmission.rateOverTime.constant;
        dashParticleEmission.rateOverTime = 0f;
        _dashParticle.Play();
        StopZeroGMoveEffects();
    }

    void FixedUpdate()
    {
        HandleAllMovement();

        if (_zeroGMoving && !_playingZeroGMoveEffects)
        {
            StartZeroGMoveEffects();
        }
        else if (!_zeroGMoving && _playingZeroGMoveEffects)
        {
            StopZeroGMoveEffects();
        }

        // _visualObject gets interpolated thanks to InterpolatedTransform
        _visualObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
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
            _oxygen.AddOxygen(_groundOxygenReplenishRate * Time.deltaTime);
        }
        else if (_inZeroGravity)
        {
            _oxygen.RemoveOxygen(_zeroGOxygenDrain * Time.deltaTime);
        }
    }

    void GetMoveDirection()
    {
        _forwardMoveDir = _cameraObject.forward * _inputManager.movementInput.y;
        _lateralMoveDir = _cameraObject.right * _inputManager.movementInput.x;
        isMoving = _forwardMoveDir != Vector3.zero || _lateralMoveDir != Vector3.zero;

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

        Vector3 accel = GetAccel();

        _zeroGMoving = false;

        if (!isGrounded && _moveDirection != Vector3.zero)
        {
            // Handle air/zeroG movement
            if (_inZeroGravity)
            {
                if (_oxygen.isEmpty)
                {
                    return;
                }
                _oxygen.RemoveOxygen(_zeroGMoveOxygenDrain * Time.deltaTime);
                accel *= _zeroGMoveMultiplier;
                _zeroGMoving = true;
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

    Vector3 GetAccel()
    {
        // Source (hehe): https://adrianb.io/2015/02/14/bunnyhop.html
        float max = isSprint ? _maxSprintSpeed : _maxSpeed;

        float projVel = Vector3.Dot(_rb.velocity, _moveDirection);
        float accelVel = moveAccel * Time.deltaTime;

        if (projVel + accelVel > max)
        {
            accelVel = max - projVel;
        }

        return _moveDirection * accelVel;
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

    public void HandleJumpInput()
    {
        if (isJumping || isDead)
        {
            return;
        }
        if (isGrounded)
        {
            HandleJumping();
            StopCoroutine(HandleGroundJumpHold());
            StartCoroutine(HandleGroundJumpHold());
        }
        else
        {
            HandleAirJumping();
        }
    }

    public void HandleJumping()
    {
        Vector3 upVelocity = Vector3.Project(_rb.velocity, _upAxis);
        Vector3 desiredUpVelocity = _upAxis * jumpVel;
        _rb.AddForce(desiredUpVelocity - upVelocity, ForceMode.VelocityChange);
    }

    void HandleAirJumping()
    {
        if (_oxygen.oxygen < _airJumpOxygenCost || isDead)
        {
            return;
        }
        _oxygen.RemoveOxygen(_airJumpOxygenCost);
        HandleJumping();
        PlayDashEffects();
    }

    IEnumerator HandleGroundJumpHold()
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

    public void HandleDashInput()
    {
        if (_oxygen.oxygen < _dashOxygenCost || isDead)
        {
            return;
        }
        _oxygen.RemoveOxygen(_dashOxygenCost);
        HandleDash();
    }

    void HandleDash()
    {
        Vector3 targetDirection = _moveDirection;
        if (targetDirection == Vector3.zero)
        {
            targetDirection = _cameraObject.forward;
        }

        // Remove velocity not in target direction
        _rb.velocity = Vector3.Project(_rb.velocity, targetDirection);
        _rb.AddForce(targetDirection.normalized * _dashVel, ForceMode.VelocityChange);
        PlayDashEffects();
    }

    void PlayDashEffects()
    {
        StopCoroutine(DashEffectCoroutine());
        StartCoroutine(DashEffectCoroutine());
        _audioSource.PlayOneShot(_dashAudio);
    }

    IEnumerator DashEffectCoroutine()
    {
        var emission = _dashParticle.emission;
        emission.rateOverTime = _dashParticleRate;
        yield return new WaitForSeconds(0.2f);
        emission.rateOverTime = 0f;
    }

    void StartZeroGMoveEffects()
    {
        _audioSource.Play();
        _playingZeroGMoveEffects = true;
        var emission = _zeroGMoveParticle.emission;
        emission.rateOverDistance = _originalRateOverDistance;
    }

    void StopZeroGMoveEffects()
    {
        _audioSource.Stop();
        var emission = _zeroGMoveParticle.emission;
        emission.rateOverDistance = 0f;
        _playingZeroGMoveEffects = false;
    }

    private bool CheckGrounded()
    {
        if (_inZeroGravity)
        {
            return false;
        }
        RaycastHit hit;

        bool result;
        result = Physics.SphereCast(transform.position + _upAxis * _spherecastStartOffset, 0.4f, -_upAxis, out hit, _spherecastDist, _playerLayerMask);
        if (result && !isGrounded)
        {
            _playerEffects.JumpDustVFX();
        }
        return result;
    }

    private Vector3 GetGravity()
    {
        _inZeroGravity = _cgrb.gravity == Vector3.zero;
        if (!_inZeroGravity)
        {
            _upAxis = _cgrb.upAxis;
        }
        return _cgrb.gravity;
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

        _rb.MoveRotation(newUpRotation);

        _upAxis = transform.up;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _upAxis);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, _moveDirection);
        Gizmos.DrawRay(transform.position + _upAxis * _spherecastStartOffset, -_upAxis * _spherecastDist);
    }
}
