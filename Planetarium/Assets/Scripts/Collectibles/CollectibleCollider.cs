using UnityEngine;

public class CollectibleCollider : MonoBehaviour
{
    private CollectibleBuff collectibleBuff;
    private CollectibleRespawn collectibleRespawn;

    private void Awake()
    {
        collectibleBuff = GetComponent<CollectibleBuff>();
        collectibleRespawn = GetComponent<CollectibleRespawn>();
    }
    
    /* When a rigidbody collides with this object. */
    private void OnTriggerEnter(Collider other)
    {
        // Give the boost only if the collider is the player.
        if (other.gameObject.CompareTag("Player"))
        {
            collectibleBuff.ApplyBuff();
            collectibleRespawn.DisableObject();
        }
    }

}
