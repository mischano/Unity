using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;

    private int horizontal;
    private int vertical;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        // animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
        // animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
    }
}
