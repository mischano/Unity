/*  Script from https://www.studica.com/blog/how-to-create-a-projectile-in-unity
 *
 *  TODO: modify to allow laser "beams" instead of spheres
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Rigidbody projectile;
    public Transform SpawnPoint;
    public float speed;             // speed of projectile

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        Rigidbody clone = (Rigidbody)Instantiate(projectile, SpawnPoint.position, projectile.rotation);
        clone.velocity = SpawnPoint.TransformDirection(Vector3.forward * speed);
    }
}
