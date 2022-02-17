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

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    public float facingDirection = 1;
    public bool isFacingRight = true;

    public bool isTouchingWall = false;
    public bool isTouchingWallAbove = false;
    public bool IsGrounded = true;

    public bool IsWalking = false;

    public float groundCheckRadius;
    public float wallCheckDistance;

    public float stopDistance;

    public float PlayerCheckRadius;

    public bool PlayerInRange = false;
    public bool PlayerWasInRange = false;

    public int EnemieAttackDamage;

    public float knockBackSpeedX = 1f;
    public float knockBackSpeedY= 2f;
    public float SpinSpeed;
    public float enemyDeathTime;
}



