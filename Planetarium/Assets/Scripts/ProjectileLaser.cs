using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLaser : MonoBehaviour
{
    public float lifetime;
    [SerializeField]
    float _damage;

    private void Awake()
    {
        Destroy(this.gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Health healthScript = collision.gameObject.GetComponent<Health>();
        DamageFlash flash = collision.gameObject.GetComponent<DamageFlash>();
        

        if (healthScript != null)
        {
            healthScript.TakeDamage(_damage);
            if (flash != null)
            {
                flash.FlashStart();
            }
            else
            {
                // child object of enemy prefab MUST be first object in tree
                DamageFlash childFlash = collision.transform.GetChild(0).GetComponent<DamageFlash>();
                childFlash.FlashStart();
            }
        }
        Destroy(this.gameObject);
    }
}
