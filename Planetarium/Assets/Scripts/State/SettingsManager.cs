using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    float _sensitivity = 3.0f;
    [SerializeField] TextMeshProUGUI _sensValueText;
    [SerializeField] Slider _slider;

    void Start()
    {
        // TODO save value between scene load (make a PersistentState class)
        SetSensitivity(_sensitivity);
        _slider.value = _sensitivity;
    }

    public void SetSensitivity(float val)
    {
        _sensitivity = val;
        GameObject.Find("AstronautSimulated")?.GetComponent<InputManager>()?.SetSensitivity(val);
        _sensValueText.text = _sensitivity.ToString("0.00");
    }
}
