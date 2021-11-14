using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using TMPro;

public class Scrap : MonoBehaviour
{
    [SerializeField]
    private OnScrapComplete _onScrapComplete = null;

    int _maxScrap;
    int _collectedScrap = 0;

    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GameObject.FindGameObjectWithTag("UIScrap").GetComponentInChildren<TextMeshProUGUI>();
        _maxScrap = GameObject.FindGameObjectsWithTag("CollectibleScrap").Length;
    }

    private void Update()
    {
        _text.text = _collectedScrap.ToString() + "/" + _maxScrap.ToString();
    }

    public void AddScrap()
    {
        if (_collectedScrap == _maxScrap)
        {
            return;
        }

        _collectedScrap += 1;
        if (_collectedScrap == _maxScrap)
        {
            if (_onScrapComplete != null)
            {
                _onScrapComplete.Invoke(true);
            }
        }
    }

    public int getScrap()
    {
        return (_collectedScrap);
    }
}
