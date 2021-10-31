using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

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
    bool _inZeroGravity;
    #endregion

    [Header("Falling Settings")]
    public float _inAirTime;
    public LayerMask _groundLayer;

    private Transform cameraObject;
    private InputManager inputManager;
    private PlayerManager playerManager;
    [Header("Visual")]
    private AnimatorManager _animatorManager;
    [SerializeField]
    private GameObject _visualObject;

    private Rigidbody _rb;
    private CinemachineFreeLook _cinemachineFree;
    private CinemachineVirtualCamera _virtualCamera;
    private Vector3 _moveDirection;
    private Vector3 _gravity;
    public bool aiming;
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        _animatorManager = _visualObject.GetComponent<AnimatorManager>();

        _rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        aiming = false;
    }

    public void HandleAllMovement()
    {
       
        _gravity = GetGravity();
        _isGrounded = CheckGrounded();
        HandleMovement();
        if (aiming == true)
        {
            HandleAimRotation();
        }
        else
        {
            HandleRotation();
        }

        // _visualObject gets interpolated thanks to InterpolatedTransform
        _visualObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    private void HandleMovement()
    {
        Vector3 forwardMoveDir = cameraObject.forward * inputManager._verticalInput;
        Vector3 lateralMoveDir = cameraObject.right * inputManager._horizontalInput;
        forwardMoveDir = ProjectDirectionOnPlane(forwardMoveDir, _upAxis);
        lateralMoveDir = ProjectDirectionOnPlane(lateralMoveDir, _upAxis);

        // If we're going too fast, don't add speed in that direction.
        float max = _isSprint ? _maxSprintSpeed : _maxSpeed;
        if (Vector3.Dot(forwardMoveDir, _rb.velocity) > max)
        {
            forwardMoveDir = Vector3.zero;
        }
        if (Vector3.Dot(lateralMoveDir, _rb.velocity) > max)
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

        _rb.AddForce(moveAccel + _gravity);

        // Prevent sliding
        if (_isGrounded &&
            (_moveDirection.sqrMagnitude < _groundDragThreshold
            || Vector3.Dot(_moveDirection, _rb.velocity) < 0f))
        {
            _rb.drag = _groundDrag;
        }
        else
        {
            _rb.drag = 0f;
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

    private void HandleAimRotation()
    {
        Vector3 targetDirection;
        targetDirection = cameraObject.forward * inputManager._verticalInput
                          + cameraObject.right * inputManager._horizontalInput;


        targetDirection = ProjectDirectionOnPlane(targetDirection, _upAxis);
        targetDirection.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, _upAxis);
        Quaternion newRotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        _rb.MoveRotation(newRotation);
        
        
        //Quaternion targetAimingRotation = Quaternion.Euler(0, cameraObject.eulerAngles.y, 0);
        //Quaternion newAimRotation = Quaternion.Lerp(transform.rotation, targetAimingRotation, _rotationSpeed);
    
        //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint((Input.mousePosition + Vector3.forward * 10f));
        //float Angle = Mathf.Atan2(transform.position.y - mouseWorldPosition.y,
            //transform.position.x - mouseWorldPosition.x) * Mathf.Rad2Deg;
        //Quaternion newRotation =  Quaternion.Euler(new Vector3(0f, 0f, Angle));
        //_rb.MoveRotation(newAimRotation);
    }

    public void HandleJumping()
    {
        if (_isGrounded)
        {
            _rb.velocity += _upAxis * _jumpVel;
            _animatorManager.animator.SetBool("isJumping", true);
            _animatorManager.PlayTargetAnimation("Jumping", false);
        }
    }

    private bool CheckGrounded()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position + _upAxis * _spherecastStartOffset, 0.4f, -_upAxis, out hit, _spherecastDist, _groundLayer);
    }

    private Vector3 GetGravity()
    {
        Vector3 gravity = CustomGravity.GetGravity(_rb.position, out _upAxis);
        if (gravity == Vector3.zero)
        {
            // In zero gravity, don't alter the player's up axis
            _upAxis = transform.up;
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
