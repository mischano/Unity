using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairOnMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // cursor not visible over scope
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if escape
        // cursor will become visible again
        if (Input.GetKeyDown((KeyCode.Escape)))
        {
            Cursor.visible = true;
        }
        transform.position = Input.mousePosition;
    }
}
