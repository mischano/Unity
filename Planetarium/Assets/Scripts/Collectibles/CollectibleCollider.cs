using UnityEngine;

public class CollectibleCollider : MonoBehaviour
{
    private CollectibleBuff collectibleBuff;
    private CollectibleRespawn collectibleRespawn;
    public AudioClip collectibleAudio;
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
            AudioSource.PlayClipAtPoint(collectibleAudio, transform.position);
            collectibleBuff.ApplyBuff();
            collectibleRespawn.DisableObject();
            
        }
    }

}
