using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour
{
    [Header("Player Oxygen")]

    [SerializeField, Range(1, 9f)]
    public int currentNumberOfOxygen = 3;

    // Max number of hearts the player can have.
    [SerializeField, Range(1, 9)]
    public int maxNumberOfOxygen = 5;

    public Image[] oxygenList;

    public PlayerOxygen oxygen;

    private void Update()
    {
        HandleOxygen();
    }

    private void HandleOxygen()
    {
        for (int i = 0; i < oxygen.oxygenList.Length; i++)
        {
            // If the number of oxygen tanks is more than the limit,
            // Set the number of tanks to the limit.
            if (oxygen.currentNumberOfOxygen > oxygen.maxNumberOfOxygen)
            {
                oxygen.currentNumberOfOxygen = oxygen.maxNumberOfOxygen;
            }

            if (i < oxygen.currentNumberOfOxygen)
            {
                oxygen.oxygenList[i].enabled = true;
            }

            else
            {
                oxygen.oxygenList[i].enabled = false;
            }
        }
    }
    public void AddOxygen(int amount)
    {
        oxygen.currentNumberOfOxygen += amount;
    }
}
