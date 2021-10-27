using UnityEngine;

public class ConsumableColliderEvent : MonoBehaviour
{
    private ConsumableBuff consumableBuff;
    private ConsumableRespawn consumableRespawn;

    private void Awake()
    {
        consumableBuff = GetComponent<ConsumableBuff>();
        consumableRespawn = GetComponent<ConsumableRespawn>();
    }
    
    /* When a rigidbody collides with this object. */
    private void OnTriggerEnter(Collider other)
    {
        // Give the boost only if the collider is the player.
        if (other.gameObject.CompareTag("Player"))
        {
            consumableBuff.ApplyBuff();
            consumableRespawn.DisableObject();
        }
    }

}
