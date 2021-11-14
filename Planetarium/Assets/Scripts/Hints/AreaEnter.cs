using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaEnter : MonoBehaviour
{
    /* Triggered when the player enters triggerable area.
     * to display hint text. 
     * Callee ShowHint.cs */
    [SerializeField]
    private OnAreaEnter _enterArea = null;

    public string displayText;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            _enterArea.Invoke(displayText);
            Destroy(gameObject);
        }
    }
}
