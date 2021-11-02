using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    public Scrap _player;
    public GameManager gameManager;
  
    // Compares Collected Scrap with Player's Max scraps to allow Game Completion.
    void OnTriggerEnter(Collider other) 
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Scrap>();
        if(_player._collectedScrap == _player.maxScrap)
        {
            gameManager.CompleteLevel();
                
        }
    }
}
