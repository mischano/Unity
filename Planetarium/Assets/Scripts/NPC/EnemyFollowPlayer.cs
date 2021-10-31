using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    private Rigidbody _rb;
    public Transform player;
    private Vector3 playerPos;
    private PlayerAttributes _playerAttributes;

    private Transform planet;
    // speed of the moving enemy
    public float speed = 1.5f;
    // enemy will follow the player if within a certain range
    public float followRadius = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        playerPos = new Vector3();
        _playerAttributes = GameObject.FindGameObjectWithTag("Player").
            GetComponent<PlayerAttributes>();
        planet = GameObject.FindWithTag("Planet").transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        //Debug.Log(playerPos);
        if ((playerPos - _rb.position).magnitude <= followRadius)
        {
            _rb.constraints = RigidbodyConstraints.None;
            followPlayer();
        }
        else if ((playerPos - _rb.position).magnitude > followRadius)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    void followPlayer()
    {
        // enemy will face the player
        transform.LookAt(player.transform, Vector3.up);
        // move enemy toward the player
        Vector3 moveDir = Vector3.MoveTowards(_rb.position, playerPos, speed * Time.deltaTime) ;
        _rb.MovePosition(moveDir);
        
        Vector3 gravityUp = (transform.position - planet.position).normalized;
        Vector3 localUp = transform.up;
        transform.rotation = Quaternion.FromToRotation(localUp, gravityUp) * transform.rotation;
        
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerAttributes.TakeDamage(1);
        }
    }
}
