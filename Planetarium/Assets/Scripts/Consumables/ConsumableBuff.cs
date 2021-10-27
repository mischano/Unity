using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableBuff : MonoBehaviour
{
    #region Boost Settings
    [Header("Boost Attributes")]

    [SerializeField, Range(0, 2)]
    public int extraJumpBoost = 1;

    [SerializeField, Range(1f, 3f)]
    public float extraSprintBoost = 1.5f;

    [SerializeField, Range(0, 2)]
    public int healthBoostAmount = 1;

    [SerializeField, Range(0, 2)]
    public int oxygenBoostAmount = 1;
    #endregion

    #region Enabled Boosts
    [Header("Currently Enabled Boosts")]
    public bool boostHealth;
    public bool boostOxygen;
    public bool boostJump;
    public bool boostSprint;
    #endregion

    private PlayerAttributes _playerAttributes;

    private void Awake()
    {
        _playerAttributes = GameObject.FindGameObjectWithTag("Player").
            GetComponent<PlayerAttributes>();
    }
    
    /* Apply the proper boost to player based on enabled boosts. */
    public void ApplyBuff()
    {
        if (boostHealth)
        {
            _playerAttributes.AddHealth(healthBoostAmount);

        }
        else if (boostOxygen)
        {
            _playerAttributes.AddOxygen(oxygenBoostAmount);
        }
        else if (boostJump)
        {
            return;
        }
        else
        {
            return;
        }

    }
}
