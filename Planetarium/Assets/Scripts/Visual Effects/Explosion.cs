using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject alien;
    public GameObject particleObject;

    public void Explode()
    {
        ParticleSystem[] _particleSystem = particleObject.GetComponentsInChildren<ParticleSystem>();
        var explosion = Instantiate(_particleSystem[0], alien.transform.position, alien.transform.rotation);
        var smoke = Instantiate(_particleSystem[1], alien.transform.position, alien.transform.rotation);

        explosion.Play();
        smoke.Play();

        // No need to destory the instantiated particle systems.
        // They are set to auto-destruction in their properties. 
    }

}
