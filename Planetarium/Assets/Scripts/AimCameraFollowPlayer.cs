using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCameraFollowPlayer : MonoBehaviour
{
    public Vector3 cameraOffset;

    public Transform followTarget;

    public Transform LookAt;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = followTarget.position + cameraOffset;
    }
}
