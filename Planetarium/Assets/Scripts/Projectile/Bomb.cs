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
        

        // Delete Bomb
        Destroy(this.gameObject);
    }
}
