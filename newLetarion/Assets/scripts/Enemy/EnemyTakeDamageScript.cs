using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamageScript : MonoBehaviour
{
    
    public int health;

    void Update()
    {
        if(health <=0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        ///Hit.Play();
        health -= damage;
        Debug.Log("damage taken");
    } 
}
