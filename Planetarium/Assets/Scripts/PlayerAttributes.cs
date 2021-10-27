using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Player health attributes */
[System.Serializable]
public class Health
{
    [Header("Player Health")]
    
    // Amount of hearts the player currently owns.
    [SerializeField, Range(1, 9f)]
    public int currentNumberOfHearts;
    
    // Max number of hearts the player can have. 
    [SerializeField, Range(1, 9)]
    public int maxNumberOfHearts;

    public Image[] heartsList;
    public Sprite fullHeart;
    public Sprite emptyHeart;
}

/* Player oxygen attributes */
[System.Serializable]
public class Oxygen
{
    [Header("Player Oxygen")]

    [SerializeField, Range(1, 9f)]
    public int currentNumberOfOxygen;
    
    // Max number of hearts the player can have. 
    [SerializeField, Range(1, 9)]
    public int maxNumberOfOxygen;

    public Image[] oxygenList;

}

public class PlayerAttributes : MonoBehaviour
{
    // Reference to heart class. 
    public Health playerHealth;

    // Reference to oxygen class
    public Oxygen oxygen;

    private void Awake()
    {
        //playerHealth.currentNumberOfHearts = 5;
        //playerHealth.maxNumberOfHearts = 5;
    }

    private void Update()
    {
        HandleHealth();
        HandleOxygen();
    }
    
    /* Handles the player health attributes */
    private void HandleHealth()
    {
        for (int i = 0; i < playerHealth.heartsList.Length; i++)
        {
            // If the number of player hearts exceeds the limit,
            // set it to max limit.
            if (playerHealth.currentNumberOfHearts > playerHealth.maxNumberOfHearts)
            {
                playerHealth.currentNumberOfHearts = playerHealth.maxNumberOfHearts;
            }
            
            // If the player owns the i(th) heart, assign the red image.
            if (i < playerHealth.currentNumberOfHearts)
            {
                playerHealth.heartsList[i].sprite = playerHealth.fullHeart;
            }

            // If the player lost the i(th) heart, assign empty image. 
            else
            {
                playerHealth.heartsList[i].sprite = playerHealth.emptyHeart;
            }
            
            // If i(th) heart is within the limit, 
            // Enable the i(th) image.
            if (i < playerHealth.maxNumberOfHearts)
            {
                playerHealth.heartsList[i].enabled = true;
            }

            // If i(th) heart is not within the limit,
            // Disable the i(th) image.
            else
            {
                playerHealth.heartsList[i].enabled = false;
            }
        }
    }

    /* Handles the player oxygen attributes */
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
}
