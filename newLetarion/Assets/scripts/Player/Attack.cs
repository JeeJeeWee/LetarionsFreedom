using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private float timeBTwAttack;
    public float startTimeBTwAttack;

    public Transform attackPos;
    public Transform attackUpPos;

    public LayerMask whatIsEnemies;
    public float attackRange;
    public float attackRangeUp;

    public int damage;
    public Animator playerAnim;
    public ParticleSystem Evaporate;
    private float Direction;


    void Update()
    {
        Direction = Input.GetAxisRaw("Vertical");

        if (timeBTwAttack <= 0)
        {
            if(Input.GetButtonDown("slas"))
            {
                if(Direction >= 1)
                {
                    playerAnim.SetTrigger("slashUp");
                }
                else if(Direction == 0)
                {
                    playerAnim.SetTrigger("slash");
                }
                else if(Direction <= -1)
                {
                    playerAnim.SetTrigger("slash");
                }
            }        
        }
        else
        {
            timeBTwAttack -= Time.deltaTime;
        }
    }

    private void AttackDamage()
    {
        timeBTwAttack = startTimeBTwAttack;
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    private void AttackDamageUp()
    {
        timeBTwAttack = startTimeBTwAttack;
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackUpPos.position, attackRangeUp, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
            enemiesToDamage[i].GetComponent<Enemy>().rbEnemy.AddForce(transform.up * 10f);
            

        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackUpPos.position, attackRangeUp);
    }

    private void CreateEvaporate()
    {
        Evaporate.Play();
        Debug.Log("Created Evaporate");
    }
}
