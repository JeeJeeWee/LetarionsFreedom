using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private GameMaster gm; 
    public ParticleSystem CheckpointParticle; 


    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("colliding");
        if (other.CompareTag("Player"))
        {
            if(gm.LastCheckPointPos.x != transform.position.x && gm.LastCheckPointPos.y != transform.position.y)
            {
                CheckpointParticle.Play();
            }

            gm.LastCheckPointPos = transform.position;
        }
    }
}
