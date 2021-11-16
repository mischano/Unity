using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMovingPlatform : MonoBehaviour
{
    [SerializeField] AnimationCurve _animationCurveX;
    [SerializeField] AnimationCurve _animationCurveY;
    [SerializeField] AnimationCurve _animationCurveZ;
    [SerializeField] float _speed = 1f;
    [SerializeField] float _scale = 1f;
    Rigidbody _rb;
    float _curveTime;
    Vector3 _originalPos;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _curveTime = 0f;
        _originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = _originalPos;
        _curveTime += _speed * Time.deltaTime;

        if (_animationCurveX != null)
        {
            pos.x += _animationCurveX.Evaluate(_curveTime) * _scale;
        }
        if (_animationCurveY != null)
        {
            pos.y += _animationCurveY.Evaluate(_curveTime) * _scale;
        }
        if (_animationCurveZ != null)
        {
            pos.z += _animationCurveZ.Evaluate(_curveTime) * _scale;
        }

        _rb.MovePosition(pos);
    }
}
