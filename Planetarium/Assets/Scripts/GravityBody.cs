using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent (typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    CustomGravity planet;
    // Start is called before the first frame update
    void Awake ()
    {
        Rigidbody _rb = GetComponent<Rigidbody>();
        planet = GameObject.FindWithTag("Planet").GetComponent<CustomGravity>();
        _rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        
}
