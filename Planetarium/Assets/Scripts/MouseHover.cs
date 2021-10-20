using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Script obtained from
 *  https://www.instructables.com/How-to-make-a-main-menu-in-Unity/
 */

public class MouseHover : MonoBehaviour
{
    public Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend.material.color = Color.white;
    }

    private void OnMouseEnter()
    {
        rend.material.color = Color.gray;
    }

    private void OnMouseExit()
    {
        rend.material.color = Color.white;
    }
}
