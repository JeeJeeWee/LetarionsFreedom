using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy" , menuName = "Enemy")]
public class enemieScriptableobjects : ScriptableObject
{
    public new string name;
   
    public float RunSpeed;
    public float SquirlJumpHight;
    public float jumpTimeSquirl;

    public float startPosX;
    public float startPosY;

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    public float facingDirection = 1;
    public bool isFacingRight = true;

    public bool isTouchingWall;
    public bool isTouchingWallAbove;
    public bool IsGrounded;

    public bool IsWalking;

    public float groundCheckRadius;
    public float wallCheckDistance;

    public float PlayerCheckRadius;

    public bool PlayerInRange;
    public bool PlayerWasInRange = false;

    public int EnemieAttackDamage;

    public movement player;

    public int health;
    public float knockBackSpeedX = 1f;
    public float knockBackSpeedY= 2f;
    public float SpinSpeed;
    public float enemyDeathTime;

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Damage to Player");
        if (other.CompareTag("Player"))
        {
            player.currentHealth -= EnemieAttackDamage;
        }
    }

    public void TakeDamage(int damage)
    {
        ///Hit.Play();
        health -= damage;
        Debug.Log("damage taken");
    }
}



