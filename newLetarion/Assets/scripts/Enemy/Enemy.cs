using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rbEnemy;

    public int health;
    public float knockBackSpeedX = 1f;
    public float knockBackSpeedY= 2f;


    public bool isFacingRight = true;


}
