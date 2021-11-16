using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    public int damage;
    public float fireRate;
    public GameObject projectile;
    public Transform projectileSpawn;
    public float speed;
    public float range;

    List<GameObject> m_lastProjectiles = new List<GameObject>();
    float m_fireTimer = 0.0f;
    private Rigidbody _rb;

    GameObject _target;

    void Start()
    {
        _target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        m_fireTimer += Time.deltaTime;

        if (Vector3.Distance(transform.position, _target.transform.position) <= range && m_fireTimer >= fireRate)
        {
            SpawnProjectile();

            m_fireTimer = 0.0f;
        }
    }

    void SpawnProjectile()
    {
        m_lastProjectiles.Clear();

        GameObject clone = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);
        _rb = clone.GetComponent<Rigidbody>();
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _rb.velocity = projectileSpawn.TransformDirection(Vector3.forward * speed);
    }
}
