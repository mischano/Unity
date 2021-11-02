using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _PlayerHealth : MonoBehaviour
{
    [Header("Player Health")]

    // Amount of hearts the player currently owns.
    [SerializeField, Range(1, 9f)]
    public int currentNumberOfHearts = 3;

    // Max number of hearts the player can have.
    [SerializeField, Range(1, 9)]
    public int maxNumberOfHearts = 5;

    // At this variable hearts will start glowing.
    [SerializeField, Range(1, 3)]
    public int glowHeartsAt = 2;

    public Image[] heartsList;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    
    private bool _isVisible;

    private void Awake()
    {
        InvokeRepeating("GlowHealth", 0, 0.5f);
    }

    private void Update()
    {
        HandleHealth();
    }
    
    /* Handles the player health attributes */
    private void HandleHealth()
    {
        for (int i = 0; i < heartsList.Length; i++)
        {
            // If the number of player hearts exceeds the limit,
            // set it to max limit.
            if (currentNumberOfHearts > maxNumberOfHearts)
            {
                currentNumberOfHearts = maxNumberOfHearts;
            }

            // If the player owns the i(th) heart, assign the red image.
            if (i < currentNumberOfHearts)
            {
                heartsList[i].sprite = fullHeart;
            }

            // If the player lost the i(th) heart, assign empty image.
            else
            {
                heartsList[i].sprite = emptyHeart;
            }

            // If i(th) heart is within the limit,
            // Enable the i(th) image.
            if (i < maxNumberOfHearts)
            {
                heartsList[i].enabled = true;
            }

            // If i(th) heart is not within the limit,
            // Disable the i(th) image.
            else
            {
                heartsList[i].enabled = false;
            }
        }
    }
    
    /* Handles changing the color of heart sprites when
     * player's health is low */
    private void GlowHealth()
    {
        // Return if the player doesn't have low health.
        if (currentNumberOfHearts > glowHeartsAt)
        {
            // Return if the first heart sprite is visible
            // *if the first sprite is visible, all others are visible.
            if (heartsList[0].color.a == 255)
            {
                return;
            }
            // Make the sprites visible, then return.
            else
            {
                Color a = heartsList[0].color;
                a.a = 255;
                for (int i = 0; i < maxNumberOfHearts; i++)
                {
                    heartsList[i].color = a;
                }
            }
        }

        // Get the current heart sprite color.
        Color currentColor = heartsList[0].color;

        // Change the alpha channel of the sprite.
        currentColor.a = _isVisible ? 0 : 255;

        // For each red heart, set its color to currentColor.
        for (int i = 0; i < currentNumberOfHearts; i++)
        {
            heartsList[i].color = currentColor;
        }

        _isVisible = !_isVisible;
    }

    /* Increases the number of hearts.
     * Called from ConsumableBuff */
    public void AddHealth(int amount)
    {
        currentNumberOfHearts += amount;
    }
    
    /*decreases the number of hearts.
     Called from EnemyFollowPlayer*/
    public void TakeDamage(int amount)
    {
        currentNumberOfHearts -= amount;
        
        if(currentNumberOfHearts == 0)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
