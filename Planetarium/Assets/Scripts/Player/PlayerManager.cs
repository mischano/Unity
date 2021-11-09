using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    InputManager inputManager;
    PlayerMovement playerMovement;


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
        animator.SetBool("isWalking", playerMovement.isWalking);
        animator.SetBool("isSprinting", playerMovement.isSprint);
        animator.SetBool("isJumping", playerMovement.isJumping);
        animator.SetBool("isFalling", playerMovement.isFalling);
    }
}
