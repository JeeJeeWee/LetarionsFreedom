using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikes : MonoBehaviour
{

    public movement player;
    public startmovement StartPlayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("colliding");
        if (other.CompareTag("Player"))
        {
            player.currentHealth = 0;
            StartPlayer.currentHealth = 0;
        }
    }
}