using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    [SerializeField]
    private AnimatorManager animatorManager;
    private PlayerMovement playerMovement;

    #region User Input WASD
    [Header("User Input (WASD)")]

    public Vector2 _movementInput;

    public float _horizontalInput;
    public float _verticalInput;
    #endregion

    #region User Input Actions
    [Header("User Input Actions")]

    public bool _leftShift;
    public bool _space;
    #endregion

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
    }

    private void HandleJumpingInput()
    {
        if (_space)
        {
            _space = false;
            playerMovement.HandleJumping();
        }
    }

    private void HandleMovementInput()
    {
        _verticalInput = _movementInput.y;
        _horizontalInput = _movementInput.x;

        float moveAmount = HandleMovementAnimationType();

        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }
    private void HandleSprintingInput()
    {
        playerMovement.isSprint = _leftShift;
    }

    private float HandleMovementAnimationType()
    {
        float keyboardInput = Mathf.Clamp01(Mathf.Abs(_horizontalInput) + Mathf.Abs(_verticalInput));
        float result;

        if (keyboardInput > 0)
        {
            if (_leftShift)
            {
                result = 1f;    // Sprint
            }
            else
            {
                result = 0.5f;  // Walk
            }
        }
        else
        {
            result = 0f;    // Idle
        }

        return result;
    }


    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            // Store user input (WASD) in _movementInput vector.
            playerControls.PlayerMovement.Movement.performed += i => _movementInput = i.ReadValue<Vector2>();

            // Store user input (Left Shit) in _leftShift bool.
            playerControls.PlayerActions.SprintPress.performed += i => _leftShift = true;
            playerControls.PlayerActions.SprintRelease.canceled += i => _leftShift = false;

            // Store user input (Space) in _space bool.
            playerControls.PlayerActions.Jump.performed += i => _space = true;
            playerControls.PlayerActions.Jump.canceled += i => _space = false;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
