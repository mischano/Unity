using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using TMPro;

public class Scrap : MonoBehaviour
{
    [SerializeField]
    private OnScrapComplete _onScrapComplete = null;

    [SerializeField]
    public int maxScrap = 3;

    public int collectedScrap = 0;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GameObject.FindGameObjectWithTag("Scrap").GetComponentInChildren<TextMeshProUGUI>();
    }
    
    private void Update()
    {
        _text.text = collectedScrap.ToString() + "/" + maxScrap.ToString();
    }

    public void AddScrap()
    {
        if (collectedScrap == maxScrap)
        {
            return;
        }

        collectedScrap += 1;
        if (collectedScrap == maxScrap)
        {
            if (_onScrapComplete != null)
            {
                _onScrapComplete.Invoke(true);
            }
        }
    }

    public int getScrap()
    {
        return(collectedScrap);
    }
}
