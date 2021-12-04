using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    private Rigidbody _rb;
    private GameObject _player;
    private Vector3 _playerPos;
    private _PlayerHealth _playerAttributes;
    private CustomGravityRigidbody _cgrb;

    // speed of the moving enemy
    public float speed = 1.5f;
    // enemy will follow the player if within a certain range
    public float followRadius = 20.0f;

    public float rotationSpeed = 700;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerAttributes = _player.GetComponent<_PlayerHealth>();
        _cgrb = GetComponent<CustomGravityRigidbody>();
    }

    void FixedUpdate()
    {
        _playerPos = _player.transform.position;
        //Debug.Log(playerPos);
        if ((_playerPos - _rb.position).magnitude <= followRadius)
        {
            _rb.constraints = RigidbodyConstraints.None;
            followPlayer();
        }
        else if ((_playerPos - _rb.position).magnitude > followRadius)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    void followPlayer()
    {
        // move enemy toward the player
        Vector3 moveDir = Vector3.MoveTowards(_rb.position, _playerPos, speed * Time.deltaTime);
        _rb.MovePosition(moveDir);
        Vector3 positionDiff = _playerPos - _rb.position;
        Vector3 projectedMoveDir = Vector3.ProjectOnPlane(positionDiff, _cgrb.upAxis).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(projectedMoveDir, _cgrb.upAxis);
        _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerAttributes.TakeDamage(1, _rb.position);
        }
    }
}
