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
    private float verticalDamp = -0.05f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        Vector3 shootDir;
        Rigidbody clone = (Rigidbody)Instantiate(projectile, SpawnPoint.position, projectile.rotation);
        shootDir = Vector3.forward * speed;
        shootDir.y = verticalDamp * speed;
        clone.velocity = SpawnPoint.TransformDirection(shootDir);
    }
}
