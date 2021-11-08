using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Experimental.VFX;

public class ProjectileLaser : MonoBehaviour
{
    public float lifetime;
    [SerializeField]
    float _damage;

    private VisualEffect ve;

    private void Awake()
    {
        Destroy(this.gameObject, lifetime);
        ve = GameObject.FindGameObjectWithTag("VFX Spark").GetComponent<VisualEffect>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided!");
        Health healthScript = collision.gameObject.GetComponent<Health>();
        DamageFlash flash = collision.gameObject.GetComponent<DamageFlash>();
        ve.transform.position = collision.contacts[0].point;
        ve.Play();
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
