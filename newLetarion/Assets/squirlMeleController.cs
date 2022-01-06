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


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        enemy.startPosX = rb.position.x;
        enemy.startPosY = rb.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SquirlMovementVoid());
        if(enemy.PlayerInRange)
        {
            enemy.PlayerWasInRange = true;
        }

        if (enemy.health <= 0)
        {
            Destroy(gameObject);
        }
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        CheckSurroundings();
    }

    IEnumerator SquirlMovementVoid()
    {
        if(enemy.PlayerWasInRange)
        {    
            if(!enemy.isTouchingWall)
            {
                rb.velocity = new Vector2(enemy.RunSpeed * enemy.facingDirection, rb.velocity.y);
                Debug.Log(enemy.RunSpeed * enemy.facingDirection);
            }
            else if(enemy.isTouchingWall && !enemy.isTouchingWallAbove)
            {
                if(enemy.IsGrounded)
                {
                    squirlJump();
                    yield return new WaitForSeconds(enemy.jumpTimeSquirl);
                }
                rb.velocity = new Vector2(enemy.RunSpeed * enemy.facingDirection, rb.velocity.y);
            }
            else if(enemy.isTouchingWall && enemy.isTouchingWallAbove)
            {
                Flip();
                rb.velocity = new Vector2(enemy.RunSpeed * enemy.facingDirection, rb.velocity.y);
            }

            if (rb.velocity.x == 0)
            {
                enemy.IsWalking = false;
            }
            else
            {
                enemy.IsWalking = true;
            }
        }
    } 

    public void squirlJump()
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
