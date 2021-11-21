using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    public PlayerControls playerControls;
    [SerializeField]
    private PlayerMovement _playerMovement;
    private PlayerShoot _playerShoot;

    #region User Input WASD
    [Header("User Input (WASD)")]

    public Vector2 movementInput;
    #endregion

    #region User Input Actions
    [Header("User Input Actions")]

    public bool jump;
    public bool fire;
    #endregion

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerShoot = GetComponent<PlayerShoot>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            SetupPlayerControls();
        }

        playerControls.Enable();
    }

    void SetupPlayerControls()
    {
        playerControls = new PlayerControls();

        // WASD: movementInput
        playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

        // Left shift: dash
        playerControls.PlayerActions.Dash.performed += i =>
        {
            if (PauseManagement.Core.PauseManager.IsPaused)
            {
                return;
            }
            _playerMovement.HandleDashInput();
        };

        // Space: jump
        playerControls.PlayerActions.Jump.performed += i =>
        {
            if (PauseManagement.Core.PauseManager.IsPaused)
            {
                return;
            }
            jump = true;
            _playerMovement.HandleJumpInput();
        };
        playerControls.PlayerActions.Jump.canceled += i => jump = false;

        // Left mouse: fire
        playerControls.PlayerActions.Fire.performed += i =>
        {
            if (PauseManagement.Core.PauseManager.IsPaused)
            {
                return;
            }
            fire = true;
            _playerShoot.HandleFiring();
        };
        playerControls.PlayerActions.Fire.canceled += i =>
        {
            fire = false;
        };
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
