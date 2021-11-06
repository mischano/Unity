using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    public PlayerControls playerControls;
    [SerializeField]
    private PlayerMovement playerMovement;

    #region User Input WASD
    [Header("User Input (WASD)")]

    public Vector2 movementInput;

    public float horizontalInput;
    public float verticalInput;
    #endregion

    #region User Input Actions
    [Header("User Input Actions")]

    public bool sprint;
    public bool jump;
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
        if (jump && !playerMovement.isJumping)
        {
            jump = false;
            playerMovement.HandleJumping();
        }
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }

    private void HandleSprintingInput()
    {
        playerMovement.isSprint = sprint;
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            // Store user input (WASD) in movementInput vector.
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            // Store user input (Left Shit) in sprint bool.
            playerControls.PlayerActions.SprintPress.performed += i => sprint = true;
            playerControls.PlayerActions.SprintRelease.canceled += i => sprint = false;

            // Store user input (Space) in jump bool.
            playerControls.PlayerActions.Jump.performed += i => jump = true;
            playerControls.PlayerActions.Jump.canceled += i => jump = false;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
