using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Updates the position and rotation of an empty GameObject for the camera to follow.
public class CameraFollowPoint : MonoBehaviour
{
    [SerializeField] Transform _followTarget;
    [SerializeField] Vector3 _offset = Vector3.zero;

    void LateUpdate()
    {
        UpdateUpRotation();
        // Rotation must be updated first for the offset to be correctly rotated.
        UpdatePosition();
    }

    void UpdatePosition()
    {
        transform.position = _followTarget.position + _followTarget.rotation * _offset;
    }

    void UpdateUpRotation()
    {
        transform.rotation = Quaternion.FromToRotation(transform.up, _followTarget.up) * transform.rotation;
    }
}
