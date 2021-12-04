using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsManager : MonoBehaviour
{
    [SerializeField] GameObject[] _toEnable;
    [SerializeField] GameObject[] _toDisable;
    [SerializeField] TMP_Text _timeText;
    [SerializeField] TMP_Text _killsText;
    [SerializeField] TMP_Text _retriesText;
    [SerializeField] TimerManager _timerManager;

    public void ShowResultsScreen()
    {
        foreach (GameObject o in _toEnable)
        {
            o.SetActive(true);
        }
        foreach (GameObject o in _toDisable)
        {
            o.SetActive(false);
        }

        _timeText.text = TimerManager.FormatTime(_timerManager.time);
        _killsText.text = LevelStats.GetInstance().kills.ToString();
        _retriesText.text = LevelStats.GetInstance().retries.ToString();
    }
}
