using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class MovingPlatform : MonoBehaviour
{
    public float _speed = 3.0f;
    private bool _active;
    private bool _resetting;
    // starting point of the platforms
    private Vector3 initialPoint;
    private Rigidbody _rb;
    // Object and position component of end spot for moving platforms
    public GameObject endSpot;
    private Vector3 endPosition;
    private void Start()
    {
        // platforms are not moving
        _active = false;
        _resetting = false;
        // intitial point of platforms 
        initialPoint = transform.position;
        _rb = GetComponent<Rigidbody>();
        // position of end platform
        endPosition = endSpot.transform.position;
    }

    private void LateUpdate()
    {
        if (_active)  // if player is on the platform
        {
            if (!_resetting) // if platform is moving TOWARD endpoint
            {
                MovePlatform();
            }
            else
            {
                ResetPosition(); // go back to initial point if resetting
            }
        }
        else
        {
            ResetPosition(); // go back to initial position if player is not on platform (after oscillation)
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _active = true;
        }
    }
    

    private void MovePlatform()
    {
        // handles movement toward the endpoint
        _resetting = false;
        Vector3 moveDir = Vector3.MoveTowards(transform.position, endPosition, _speed * Time.deltaTime);
        _rb.MovePosition(moveDir);
        
        if (transform.position == endPosition)
        {
            _resetting = true;
            ResetPosition();
        }
      
    }

    private void ResetPosition()
    {
        // reset platform to original position
        Vector3 moveDir = Vector3.MoveTowards(transform.position, initialPoint, _speed * Time.deltaTime);
        _rb.MovePosition(moveDir);
        
        if (transform.position == initialPoint)
        {
            MovePlatform();
        }
    }
}
