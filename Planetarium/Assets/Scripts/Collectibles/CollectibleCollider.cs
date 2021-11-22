using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CollectibleCollision : UnityEvent<bool> { }

public class CollectibleCollider : MonoBehaviour
{
    public AudioClip collectibleAudio;
    private CollectibleBuff _collectibleBuff;

    [SerializeField]
    private UnityEvent _collectibleCollision = null;

    private void Awake()
    {
        _collectibleBuff = GetComponent<CollectibleBuff>();
        
    }
    
    /* When a rigidbody collides with this object. */
    private void OnTriggerEnter(Collider other)
    {
        // Give the boost only if the collider is the player.
        if (other.gameObject.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(collectibleAudio, other.gameObject.transform.position);
            _collectibleBuff.ApplyBuff();
            
            // Set collected to true for listening event.
            if (_collectibleCollision != null)
            {
                _collectibleCollision.Invoke();
            }

            Destroy(gameObject);
        }
    }

}
