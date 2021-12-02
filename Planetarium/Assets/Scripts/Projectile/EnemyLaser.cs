using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Experimental.VFX;

public class EnemyLaser : MonoBehaviour
{
    public float lifetime;
    [SerializeField]
    float _damage;

    private VisualEffect ve;
    private GameObject _player;
    private _PlayerHealth _playerAttributes;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerAttributes = _player.GetComponent<_PlayerHealth>();
        Destroy(this.gameObject, lifetime);
        ve = GameObject.FindGameObjectWithTag("VFX Spark").GetComponent<VisualEffect>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Health healthScript = collision.gameObject.GetComponent<Health>();
        DamageFlash flash = collision.gameObject.GetComponent<DamageFlash>();

        /* I couldn't destroy instantiated VFX properly,
         * so I am having an active VFX the entire time,
         * and just transforming it's position :(
         * Hopefully, I will fix this issue.  */
        ve.transform.position = collision.contacts[0].point;
        ve.Play();

        if (collision.gameObject.CompareTag("Player"))
        {
            _playerAttributes.TakeDamage(1);
        }
        Destroy(this.gameObject);
    }
}
