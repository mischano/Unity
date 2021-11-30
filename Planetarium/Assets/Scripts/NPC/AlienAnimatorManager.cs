using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAnimatorManager : MonoBehaviour
{
    [SerializeField] Animator _animator;
    private AlienShooter _alien;
    // Start is called before the first frame update
    void Start()
    {
        _alien = GetComponent<AlienShooter>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _animator.SetBool("isWalking", _alien.isWalking);
        _animator.SetBool("isShooting", _alien.isShooting);
        //_animator.SetBool("isDead", _alien.isDead);
    }
}
