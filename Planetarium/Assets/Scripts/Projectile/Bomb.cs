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

    public GameObject explosionEffect;
    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
        hasExploded = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Countdown();
    }

    private void Countdown()
    {
        countdown -= Time.deltaTime;
        if (countdown == 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    private void Explode()
    {
        Debug.Log("Boom");
        // Show effect
        Instantiate(explosionEffect, transform.position, transform.rotation);
        // Damage Nearby Objects
        

        // Delete Bomb
        Destroy(this.gameObject);
    }
}
