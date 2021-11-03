using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEnlarge : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private int _remainingTime; 

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }


}
