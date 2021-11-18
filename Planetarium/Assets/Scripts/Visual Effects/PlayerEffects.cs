using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerEffects : MonoBehaviour
{
    public GameObject dust;
    public GameObject origin;

    private VisualEffect visualEffect;

    public void JumpDustVFX() 
    {
        var clone = Instantiate(dust, origin.transform.position, origin.transform.rotation);
        visualEffect = clone.GetComponent<VisualEffect>();
        visualEffect.Play();
        
        Destroy(clone, 1.5f);
    }
}
