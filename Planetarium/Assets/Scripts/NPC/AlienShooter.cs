using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class AlienShooter : MonoBehaviour
{
    #region Animation Flags
    public bool isWalking;

    public bool isShooting;
    public bool isDead;
    #endregion
    
    // for shooting
    [SerializeField] GameObject _projectile;
    [SerializeField] Transform _spawnPoint;
    private Rigidbody _bombRb;
    private float projectileSpeed = 10f;
    
    // astronaut attributes
    private GameObject _player;
    private Vector3 _playerPos;
    
    // Rigidbody
    private CustomGravityRigidbody _cgrb;
    private Rigidbody _rb;
    
    // speed of the moving enemy
    public float speed = 3.5f;
    public float rotationSpeed = 700;

    // enemy will follow or shoot the player if within a certain range
    public float followRadius = 20.0f;
    public float shootRadius = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        isWalking = false;
        isShooting = false;
        _rb = GetComponentInChildren<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _cgrb = GetComponentInChildren<CustomGravityRigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _playerPos = _player.transform.position;
        
        if (!isShooting && !isWalking && ((_playerPos - _rb.position).magnitude <= followRadius))
        {
            //_rb.constraints = RigidbodyConstraints.None;
            WalkToTarget();
            
        }
        else 
        {
            isWalking = false;
            //_rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void WalkToTarget()
    {
        // move enemy toward the player
        isWalking = true;
        Move();
        
        // if player is within the shoot radius, stop walking and shoot
        if ( !isShooting && (_playerPos - _rb.position).magnitude < shootRadius)
        {
            isWalking = false;
            isShooting = true;
            Invoke("Shoot", 1.0f);
        }
    }

    private void Move()
    {
        Vector3 moveDir = Vector3.MoveTowards(_rb.position, _playerPos, speed * Time.deltaTime);
        _rb.MovePosition(moveDir);
        Vector3 positionDiff = _playerPos - _rb.position;
        Vector3 projectedMoveDir = Vector3.ProjectOnPlane(positionDiff, _cgrb.upAxis).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(projectedMoveDir, _cgrb.upAxis);
        _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        
    }

   
    private void Shoot()
    {
       
        //isWalking = false;
        //isShooting = true;
        
        GameObject clone = Instantiate(_projectile, _spawnPoint.position, Quaternion.identity);
        clone.tag = "Bomb";
        _bombRb = clone.GetComponent<Rigidbody>();
        _bombRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Vector3 direction = _playerPos - _spawnPoint.position;
        _bombRb.AddForce(direction * projectileSpeed, ForceMode.Impulse);
        Invoke("ShootCoolDown", 5.0f);

    }

    private void ShootCoolDown()
    {
        isShooting = false;
    }
}
