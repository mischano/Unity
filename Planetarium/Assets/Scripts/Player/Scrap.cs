using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scrap : MonoBehaviour
{
    [SerializeField]
    public int maxScrap = 3;

    private int _collectedScrap = 0;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GameObject.FindGameObjectWithTag("Scrap").GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        _text.text = _collectedScrap.ToString() + "/" + maxScrap.ToString();
    }

    public void AddScrap(int amount)
    {
        _collectedScrap += amount;
    }
}
