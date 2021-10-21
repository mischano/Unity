using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    GameObject[] tags;
    GameObject c;

    private void Awake()
    {
        c = GameObject.FindWithTag("Canvas");
        c.SetActive(false);
    }

    private void Update()
    {
        tags = GameObject.FindGameObjectsWithTag("Collectibles");        
        for (int i = 0; i < tags.Length; i++)
        {
            MeshRenderer m = tags[i].GetComponent<MeshRenderer>();
            if (m.enabled)
            {
                return;
            }
        }

        Time.timeScale = 0f;
        c.SetActive(true);
    }
}
