using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBuff : MonoBehaviour
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

    [SerializeField, Range(0, 2)]
    public int scrapBoostAmount = 1;
    #endregion

    #region Enabled Boosts
    [Header("Currently Enabled Boosts")]
    public bool boostHealth;
    public bool boostOxygen;
    public bool boostScrap;
    public bool boostJump;
    public bool boostSprint;
    #endregion

    private _PlayerHealth _playerHealth;
    private Scrap _scrap;
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        
        _playerHealth = _player.GetComponent<_PlayerHealth>();
        _scrap = _player.GetComponent<Scrap>();
    }
    
    /* Apply the proper boost to player based on enabled boosts. */
    public void ApplyBuff()
    {
        if (boostHealth)
        {
            _playerHealth.AddHealth(healthBoostAmount);

        }
        //if (boostOxygen)
        //{
        //    _playerHealth.AddOxygen(oxygenBoostAmount);
        //}
        //if (boostScrap)
        //{
        //    _scrap.AddScrap(scrapBoostAmount);
        //}
    }
}
