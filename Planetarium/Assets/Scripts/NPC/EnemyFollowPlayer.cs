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
    private Transform _planet;

    // speed of the moving enemy
    public float speed = 1.5f;

    // enemy will follow the player if within a certain range
    public float followRadius = 20.0f;

    public float rotationSpeed = 300f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerAttributes = _player.GetComponent<_PlayerHealth>();
        _planet = GameObject.FindWithTag("Planet").transform;
    }

    // Update is called once per frame
        void Update()
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
            // enemy will face the player
            transform.LookAt(_player.transform, Vector3.up);
            // move enemy toward the player
            Vector3 moveDir = Vector3.MoveTowards(_rb.position, _playerPos, speed * Time.deltaTime);
            _rb.MovePosition(moveDir);
            Vector3 gravityUp = (transform.position - _planet.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, _planet.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _playerAttributes.TakeDamage(1);
            }
        }
    
}
