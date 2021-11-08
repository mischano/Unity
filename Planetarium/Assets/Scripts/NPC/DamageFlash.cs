using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private MeshRenderer _render;

    private Color color;

    private float _flashTime = 0.15f;

    private Color origColor;

    public AudioClip DamageSFX;
    // Start is called before the first frame update
    void Start()
    {
        _render = GetComponent<MeshRenderer>();
        
    }

    public void DamageFX()
    {
        
        AudioSource.PlayClipAtPoint(DamageSFX, gameObject.transform.position);
        FlashStart();
    }
    
    public void FlashStart()
    {
        origColor = _render.material.color;
        _render.material.color = Color.red;
        Invoke("FlashStop", _flashTime);
    }

    void FlashStop()
    {
        _render.material.color = origColor;
    }
}
