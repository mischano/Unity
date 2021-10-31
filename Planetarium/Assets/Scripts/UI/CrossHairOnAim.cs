using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CrossHairOnAim : MonoBehaviour
{
    private Image crossHair;

    private void Awake()
    {
        gameObject.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    // will update the crosshair based on aiming setting
    void Update()
    {
       
        if (Input.GetButton("Fire2"))
        {
           
            // if the player is aiming, display the corsshair
            gameObject.GetComponent<Image>().enabled = true;
        }
        else
        {
            // if not aiming, do not display
            gameObject.GetComponent<Image>().enabled = false;
        }
    }
}
