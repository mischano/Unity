using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Player health attributes */
//[System.Serializable]
//public class PlayerHealth
//{
//    [Header("Player Health")]

//    // Amount of hearts the player currently owns.
//    [SerializeField, Range(1, 9f)]
//    public int currentNumberOfHearts = 3;

//    // Max number of hearts the player can have.
//    [SerializeField, Range(1, 9)]
//    public int maxNumberOfHearts = 5;

//    // At this variable hearts will start glowing.
//    [SerializeField, Range(1, 3)]
//    public int glowHeartsAt = 2;

//    public Image[] heartsList;
//    public Sprite fullHeart;
//    public Sprite emptyHeart;
//}

/* Player oxygen attributes */
[System.Serializable]
public class PlayerOxygen
{
    [Header("Player Oxygen")]

    [SerializeField, Range(1, 9f)]
    public int currentNumberOfOxygen = 3;

    // Max number of hearts the player can have.
    [SerializeField, Range(1, 9)]
    public int maxNumberOfOxygen = 5;

    public Image[] oxygenList;

}

public class PlayerAttributes : MonoBehaviour
{
    // Reference to heart class.
    // public PlayerHealth health;

    // Reference to oxygen class
    public PlayerOxygen oxygen;

    // private bool isSpriteVisible;

    //private void Awake()
    //{
    //    //health.currentNumberOfHearts = 5;
    //    //health.maxNumberOfHearts = 5;

    //    InvokeRepeating("GlowHealth", 0, 0.5f);
    //}

    private void Update()
    {
        // HandleHealth();
        HandleOxygen();
    }

    /* Handles the player health attributes */
    //private void HandleHealth()
    //{
    //    for (int i = 0; i < health.heartsList.Length; i++)
    //    {
    //        // If the number of player hearts exceeds the limit,
    //        // set it to max limit.
    //        if (health.currentNumberOfHearts > health.maxNumberOfHearts)
    //        {
    //            health.currentNumberOfHearts = health.maxNumberOfHearts;
    //        }

    //        // If the player owns the i(th) heart, assign the red image.
    //        if (i < health.currentNumberOfHearts)
    //        {
    //            health.heartsList[i].sprite = health.fullHeart;
    //        }

    //        // If the player lost the i(th) heart, assign empty image.
    //        else
    //        {
    //            health.heartsList[i].sprite = health.emptyHeart;
    //        }

    //        // If i(th) heart is within the limit,
    //        // Enable the i(th) image.
    //        if (i < health.maxNumberOfHearts)
    //        {
    //            health.heartsList[i].enabled = true;
    //        }

    //        // If i(th) heart is not within the limit,
    //        // Disable the i(th) image.
    //        else
    //        {
    //            health.heartsList[i].enabled = false;
    //        }
    //    }
    //}

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

    /* Handles changing the color of heart sprites when
     * player's health is low */
    //private void GlowHealth()
    //{
    //    // Return if the player doesn't have low health.
    //    if (health.currentNumberOfHearts > health.glowHeartsAt)
    //    {
    //        // Return if the first heart sprite is visible
    //        // *if the first sprite is visible, all others are visible.
    //        if (health.heartsList[0].color.a == 255)
    //        {
    //            return;
    //        }
    //        // Make the sprites visible, then return.
    //        else
    //        {
    //            Color a = health.heartsList[0].color;
    //            a.a = 255;
    //            for (int i = 0; i < health.maxNumberOfHearts; i++)
    //            {
    //                health.heartsList[i].color = a;
    //            }
    //        }
    //    }

    //    // Get the current heart sprite color.
    //    Color currentColor = health.heartsList[0].color;

    //    // Change the alpha channel of the sprite.
    //    currentColor.a = isSpriteVisible ? 0 : 255;

    //    // For each red heart, set its color to currentColor.
    //    for (int i = 0; i < health.currentNumberOfHearts; i++)
    //    {
    //        health.heartsList[i].color = currentColor;
    //    }

    //    isSpriteVisible = !isSpriteVisible;
    //}

    /* Increases the number of hearts.
     * Called from ConsumableBuff */
    //public void AddHealth(int amount)
    //{
    //    health.currentNumberOfHearts += amount;
    //}

    /* Increases the number of oxygen tanks.
     * Called from ConsumableBuff */
    public void AddOxygen(int amount)
    {
        oxygen.currentNumberOfOxygen += amount;
    }

    /*decreases the number of hearts.
     Called from EnemyFollowPlayer*/
    //public void TakeDamage(int amount)
    //{
    //    health.currentNumberOfHearts -= amount;
    //}
}
