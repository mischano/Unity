using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Experimental.GlobalIllumination;

public class Bomb : MonoBehaviour
{
    private CustomGravityRigidbody _cgrb;

    private Rigidbody _rb;
    public float expolodeRadius = 5.0f;
    private Vector3 explodeForce;
    private float explodeForceFactor = 15f;
    public float delay = 3.0f;
    private float countdown;
    private bool hasExploded;
    private bool landed;

    [SerializeField]
    GameObject explosionEffect;

    private MeshRenderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
        hasExploded = false;
        landed = false;
        _renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (landed)
        {
            Countdown();
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        landed = true;
    }

    private void Countdown()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 1f && !hasExploded)
        {
            _renderer.material.color = Color.red;
            if (countdown <= 0f)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        // Show effect
        Instantiate(explosionEffect, transform.position, transform.rotation);
        // Damage Nearby Objects
        GameObject _player = GameObject.FindWithTag("Player");

        if (_player != null && (_player.transform.position - transform.position).magnitude <= expolodeRadius)
        {
            _PlayerHealth playerHealth = _player.GetComponent<_PlayerHealth>();
            Rigidbody _playerRB = _player.GetComponent<Rigidbody>();
            explodeForce = (_player.transform.position - transform.position).normalized * explodeForceFactor;
            explodeForce.y = explodeForce.y + 15f;
            playerHealth.TakeDamage(1);
            _playerRB.AddForce(explodeForce, ForceMode.Impulse);
        }

        // Delete Bomb
        Destroy(this.gameObject);
    }
}
