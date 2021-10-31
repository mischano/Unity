using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    InputManager inputManager;
    PlayerMovement playerMovement;

    public bool _isInteracting;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        // TODO: When we make a pause menu, handle cursor locking there.
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerMovement.HandleAllMovement();
    }

    private void LateUpdate()
    {
        _isInteracting = animator.GetBool("isInteracting");
        playerMovement._isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerMovement._isGrounded);
    }
}
