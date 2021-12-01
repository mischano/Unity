using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public float sensitivity = 3.0f;
    [SerializeField] TextMeshProUGUI _sensValueText;
    [SerializeField] Slider _slider;

    void Start()
    {
        sensitivity = PersistentState.GetMouseSensitivity();
        SetSensitivity(sensitivity);
        _slider.value = sensitivity;
    }

    public void SetSensitivity(float val)
    {
        sensitivity = val;
        PersistentState.SetMouseSensitivity(sensitivity);

        GameObject.Find("AstronautSimulated")?.GetComponent<InputManager>()?.SetSensitivity(sensitivity);
        _sensValueText.text = sensitivity.ToString("0.00");
    }

    public void SetCheatsEnabled(bool val)
    {
        PersistentState.GetInstance().cheatsEnabled = val;
    }

    public void SetCheatGodmode(bool val)
    {
        PersistentState.GetInstance().cheatGodmode = val;
    }

    public void SetCheatInfOxygen(bool val)
    {
        PersistentState.GetInstance().cheatInfOxygen = val;
    }

    public void CheatAddScrap()
    {
        if (!PersistentState.CheatsEnabled())
        {
            return;
        }
        GameObject.Find("AstronautSimulated")?.GetComponent<Scrap>().AddScrap();
    }
}
