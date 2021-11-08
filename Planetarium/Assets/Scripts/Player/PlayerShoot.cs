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
    private AudioSource _laserSFX;
    const float INFINITY = 100f;

    public bool isDead;
    private bool fireInput;

    private void Awake()
    {
        _laserSFX = GetComponent<AudioSource>();
        isDead = false;
        fireInput = false;
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
        Ray crosshairRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;
        Vector3 targetPoint, direction;
        if (Physics.Raycast(crosshairRay, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            // We aren't aiming at anything, so aim at a point far away.
            targetPoint = Camera.main.transform.position + crosshairRay.direction * INFINITY;
        }

        direction = targetPoint - _spawnPointParent.position;

        _spawnPointParent.rotation = Quaternion.LookRotation(direction);

        GameObject clone = Instantiate(_projectile, _spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        clone.GetComponent<Rigidbody>().velocity = (targetPoint - _spawnPoint.position).normalized * _speed;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_spawnPoint.position, 0.02f);
    }
}
