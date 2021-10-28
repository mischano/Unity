using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    private Rigidbody _rb;
    public Transform player;
    private Vector3 playerPos;
    
    public float speed = 1.5f;

    public float followRadius = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //player = GameObject.FindWithTag("Player").transform;
        playerPos = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        //Debug.Log(playerPos);
        if ((playerPos - _rb.position).magnitude <= followRadius)
        {
            followPlayer();
        }
    }

    void followPlayer()
    {
        transform.LookAt(player.transform, Vector3.up);
        //Vector3 moveDir = (-_rb.position - playerPos).normalized;
        
        //Debug.Log(moveDir);
        //_rb.AddForce(new Vector3(moveDir.x, 0, moveDir.z)*speed);
        Vector3 moveDir = Vector3.MoveTowards(_rb.position, playerPos, speed * Time.deltaTime) ;
        _rb.MovePosition(moveDir);
        
        
    }
}
