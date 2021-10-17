using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerMovement playerMovement;
    AnimatorManager animatorManager;

    [Header("Movements")]
    public float horizontalInput;
    public float verticalInput;

    [Header("Actions")]
    public bool leftShift;
    public bool space;

    public Vector2 movementInput;
    
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animatorManager = GetComponent<AnimatorManager>();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        float moveAmount = HandleMovementAnimationType();
        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }
    
    private float HandleMovementAnimationType()
    {
        float keyboardInput = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        float result;

        if (keyboardInput > 0)
        {
            if (leftShift)
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

    private void HandleSprintingInput()
    {
        playerMovement.isSprint = leftShift;
    }

    private void HandleJumpingInput()
    {
        if (space)
        {
            space = false;
            playerMovement.HandleJumping();
        }
    }
    
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            
            // WASD
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            
            // LEFT SHIFT
            playerControls.PlayerActions.SprintPress.performed += i => leftShift = true;
            playerControls.PlayerActions.SprintRelease.canceled += i => leftShift = false;
            
            // SPACE 
            playerControls.PlayerActions.Jump.performed += i => space = true;
            playerControls.PlayerActions.Jump.canceled += i => space = false;
        }

        playerControls.Enable();
    }
    
    private void OnDisable()
    {
        playerControls.Disable();
    }
}
