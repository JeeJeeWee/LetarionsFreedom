using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rbEnemy;

    public int health;
    public float knockBackSpeedX = 1f;
    public float knockBackSpeedY= 2f;
    public float SpinSpeed;
    public float enemyDeathTime;

    public bool isFacingRight = true;

    ///public ParticleSystem Hit;


    // Start is called before the first frame update
    void Start()
    {
        rbEnemy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            StartCoroutine(Enemydeath());
        }
    }

    IEnumerator Enemydeath()
    {
        rbEnemy.velocity= new Vector2(knockBackSpeedX, knockBackSpeedY);
        rbEnemy.AddTorque(SpinSpeed, ForceMode2D.Impulse);
        yield return new WaitForSeconds(enemyDeathTime);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        ///Hit.Play();
        health -= damage;
        Debug.Log("damage taken");
    }

}
