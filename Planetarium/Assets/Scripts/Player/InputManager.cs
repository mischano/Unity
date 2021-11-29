using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using Cinemachine;

public class InputManager : MonoBehaviour
{
    public PlayerControls playerControls;
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
    public bool dash;
    #endregion

    [SerializeField]
    CinemachineFreeLook _cam;

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
            dash = true;
        };
        playerControls.PlayerActions.Dash.canceled += i =>
        {
            dash = false;
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

    public void SetSensitivity(float val)
    {
        const float xAxisMultiplier = 300f;
        const float yAxisMultiplier = 1f;

        _cam.m_XAxis.m_MaxSpeed = val * xAxisMultiplier;
        _cam.m_YAxis.m_MaxSpeed = val * yAxisMultiplier;
    }
}
