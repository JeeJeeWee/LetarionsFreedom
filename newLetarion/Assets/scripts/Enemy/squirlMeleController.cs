using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squirlMeleController : MonoBehaviour
{

    public enemieScriptableobjects enemy; 

    private Rigidbody2D rb;
    private Animator anim;

    public Transform groundCheck;
    public Transform wallCheck;
    public Transform wallCheck2;

    public Transform PlayerCheck;

    public Transform TargetPlayer;

    public movement player;

    public float startPosX;
    public float startPosY;
    public float rbX;
    public float rbY;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        startPosX = rb.position.x;
        startPosY = rb.position.y;
        rbX = rb.velocity.x;
        rbY = rb.velocity.y;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SquirlMovementVoid());
        if(enemy.PlayerInRange)
        {
            enemy.PlayerWasInRange = true;
        }
        
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        CheckSurroundings();
    }

    IEnumerator SquirlMovementVoid()
    {
        //enemy.PlayerWasInRange
        if(0==0)
        {   
            var TargetPlayerAxis = new Vector2(TargetPlayer.position.x, transform.position.y);

            

            if(Vector2.Distance(transform.position, TargetPlayerAxis) > enemy.stopDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, TargetPlayerAxis, enemy.RunSpeed * Time.deltaTime);
 
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if(!enemy.isTouchingWall)
            {
                //rb.velocity = new Vector2(enemy.RunSpeed * enemy.facingDirection, rb.velocity.y);
            }
            else if(enemy.isTouchingWall && !enemy.isTouchingWallAbove)
            {
                if(enemy.IsGrounded)
                {
                    squirlJump();
                    yield return new WaitForSeconds(enemy.jumpTimeSquirl);
                }
                //rb.velocity = new Vector2(enemy.RunSpeed * enemy.facingDirection, rb.velocity.y);
            }
            else if((enemy.isTouchingWall && enemy.isTouchingWallAbove))
            {
                Flip();
                //rb.velocity = new Vector2(enemy.RunSpeed * enemy.facingDirection, rb.velocity.y);
            }

            if (rb.velocity.x > 0 )
            {
                enemy.IsWalking = true;
            }
            else
            {
                enemy.IsWalking = false;
            }
        }
    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Damage to Player");
        if (other.CompareTag("Player"))
        {
            player.currentHealth -= enemy.EnemieAttackDamage;
        }
    }

    private void squirlJump()
    {
            rb.velocity = new Vector2(0, enemy.SquirlJumpHight);
    }

    public void UpdateAnimations()
    {
        anim.SetBool("isgrounded", enemy.IsGrounded);
        anim.SetBool("iswalking", enemy.IsWalking);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void Flip()
    {
        enemy.facingDirection *= -1;
        enemy.isFacingRight = !enemy.isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    private void CheckSurroundings()
    {
        enemy.IsGrounded = Physics2D.OverlapCircle(groundCheck.position, enemy.groundCheckRadius, enemy.whatIsGround);

        enemy.PlayerInRange = Physics2D.OverlapCircle(PlayerCheck.position, enemy.PlayerCheckRadius, enemy.whatIsPlayer);

        enemy.isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, enemy.wallCheckDistance, enemy.whatIsGround);
        enemy.isTouchingWallAbove = Physics2D.Raycast(wallCheck2.position, transform.right, enemy.wallCheckDistance, enemy.whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, enemy.groundCheckRadius);

        Gizmos.DrawWireSphere(PlayerCheck.position, enemy.PlayerCheckRadius);


        if (enemy.isFacingRight)
        {
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + enemy.wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
            Gizmos.DrawLine(wallCheck2.position, new Vector3(wallCheck2.position.x + enemy.wallCheckDistance, wallCheck2.position.y, wallCheck2.position.z));


        }
        else if (!enemy.isFacingRight)
        {
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x - enemy.wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
            Gizmos.DrawLine(wallCheck2.position, new Vector3(wallCheck2.position.x - enemy.wallCheckDistance, wallCheck2.position.y, wallCheck2.position.z));

        }
    }
}
