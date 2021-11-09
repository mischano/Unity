using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    public int damage;
    public float fireRate;
    public float fieldOfView;
    public GameObject projectile;
    public GameObject target;
    public Transform projectileSpawn;
    public float speed;

    List<GameObject> m_lastProjectiles = new List<GameObject>();
    float m_fireTimer = 0.0f;
    private Rigidbody _rb;

    // Update is called once per frame
    void Update()
    {
        m_fireTimer += Time.deltaTime;

        if (m_fireTimer >= fireRate)
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
