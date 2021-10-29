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
        if (healthScript != null)
        {
            healthScript.TakeDamage(_damage);
        }
        Destroy(this.gameObject);
    }
}
