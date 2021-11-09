/*  Script from https://www.studica.com/blog/how-to-create-a-projectile-in-unity
 *
 *  TODO: modify to allow laser "beams" instead of spheres
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] GameObject _projectile;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] Transform _spawnPointParent;
    [SerializeField] float _speed;             // speed of projectile

    [SerializeField] LayerMask _playerLayer;
    LayerMask _notPlayer;

    private AudioSource _laserSFX;
    private Rigidbody _rb;

    [SerializeField] float _aimNothingDistance = 100f;

    public bool isDead;
    private bool fireInput;
    Vector3 _aimTarget;

    private void Awake()
    {
        _laserSFX = GetComponent<AudioSource>();
        isDead = false;
        fireInput = false;
        _notPlayer = ~_playerLayer;
    }

    private void Update()
    {
        fireInput |= Input.GetButtonDown("Fire1");
    }

    private void FixedUpdate()
    {
        if (fireInput && !isDead)
        {
            fireInput = false;
            FireProjectile();
            _laserSFX.Play();
        }
    }

    void FireProjectile()
    {
        GetAimTarget();

        Vector3 direction = _aimTarget - _spawnPointParent.position;

        _spawnPointParent.rotation = Quaternion.LookRotation(direction);

        GameObject clone = Instantiate(_projectile, _spawnPoint.position, Quaternion.identity);
        _rb = clone.GetComponent<Rigidbody>();
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _rb.velocity = (_aimTarget - _spawnPoint.position).normalized * _speed;
    }

    void GetAimTarget()
    {
        Ray crosshairRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(crosshairRay, out hit, Mathf.Infinity, _notPlayer))
        {
            _aimTarget = hit.point;
        }
        else
        {
            // We aren't aiming at anything, so aim at a point far away.
            _aimTarget = Camera.main.transform.position + crosshairRay.direction * _aimNothingDistance;
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(_spawnPoint.position, 0.02f);
    //}
}
