using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetsOnRespawn : MonoBehaviour
{
    private Vector3 _initialPosition;
    // Serialized field used to set the moving platform to not moving when respawn
    [SerializeField] private MovingPlatform platform;
    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
    }

    public void ResetOnRespawn()
    {
        transform.position = _initialPosition;
        platform.SetMovementInactive();
    }
}
