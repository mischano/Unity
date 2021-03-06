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
    [SerializeField] Transform _headAimTarget;
    [SerializeField] Transform _armAimTarget;
    [SerializeField] float _speed;             // speed of projectile

    [SerializeField] LayerMask _playerLayer;
    LayerMask _notPlayer;

    [SerializeField] AudioClip _laserSfx;
    private AudioSource _audioSource;
    private Rigidbody _rb;

    [SerializeField] float _aimNothingDistance = 100f;

    public bool isDead;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        isDead = false;
        _notPlayer = ~_playerLayer;
    }

    private void Update()
    {
        UpdateAimTargetPos();
    }

    public void HandleFiring()
    {
        if (CanFire())
        {
            FireProjectile();
            _audioSource.PlayOneShot(_laserSfx);
        }
    }

    bool CanFire()
    {
        return !isDead;
    }

    void FireProjectile()
    {
        GameObject clone = Instantiate(_projectile, _spawnPoint.position, Quaternion.identity);
        clone.tag = "Bullet";
        _rb = clone.GetComponent<Rigidbody>();
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Vector3 direction = _headAimTarget.position - _spawnPoint.position;
        _rb.velocity = direction.normalized * _speed;
    }

    void UpdateAimTargetPos()
    {
        Ray crosshairRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;
        Vector3 aimNothingPos = Camera.main.transform.position + crosshairRay.direction * _aimNothingDistance;

        if (Physics.Raycast(crosshairRay, out hit, Mathf.Infinity, _notPlayer))
        {
            _headAimTarget.position = hit.point;
        }
        else
        {
            // We aren't aiming at anything, so aim at a point far away.
            _headAimTarget.position = aimNothingPos;
        }
        _armAimTarget.position = aimNothingPos;
    }
}
