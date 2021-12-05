using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startmovement : MonoBehaviour
{
    private GameMaster gm;

    public float movementInputDirection;
    public float movementInputDirectionY;

    private float jumpTimer;

    private Rigidbody2D rb;
    private Animator anim;

    public int MaxHealth = 10;
    public int currentHealth;

    public float facingDirection = 1;
    public float movementSpeed = 10f;
    public float Jumpforce = 16.0f;
    public float groundCheckRadius;
    public float CheckPointCheckRadius;
    public float jumpHightMultiplier = 0.5f;
    public float jumpTimerSet = 0.20f;
    public float wallCheckDistance;

    public GameObject Skull;

    public Transform groundCheck;
    public Transform CheckpointCheck;
    public Transform wallCheck;

    public bool isWalking;
    public bool isFacingRight = true;
    public bool isGrounded;
    public bool canJump;
    public bool isAttemptingToJump;
    public bool isTouchingWall;
    private bool canFlip = true;
    private bool CheckJumpMultiplier;

    public LayerMask whatIsGround;

    public ParticleSystem Dust;
    public ParticleSystem Death;

    public SlowMotion slowMotion;

    public owlManager OM;
    public DialogManager DM;

    public float GameOverTime;
    public float deathParticalTime;

    void Start()
    {

        Skull.SetActive(false);
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.LastCheckPointPos;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = MaxHealth;
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckJump();
        StartCoroutine(CheckHealth());
    }

    IEnumerator CheckHealth()
    {

        if(currentHealth <= 0)
        {
            
            rb.velocity = new Vector2(0,0);
            Death.Play();
            yield return new WaitForSeconds(deathParticalTime);
            Skull.SetActive(true);
            yield return new WaitForSeconds(GameOverTime);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        movementInputDirectionY = Input.GetAxisRaw("Vertical");


        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if (CheckJumpMultiplier && !Input.GetButton("Jump"))
        {
            CheckJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpHightMultiplier);
        }
    }


    private void ApplyMovement()
    {
         rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
    }    

    private void CheckJump()
    {
        if (jumpTimer > 0)
        {
            if (isGrounded)
            {
                NormalJump();
            }
        }

        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    private void NormalJump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, Jumpforce);
            Dust.Play();
            jumpTimer = 0;
            isAttemptingToJump = false;
            CheckJumpMultiplier = true;
        }
    }


    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    private void CheckMovementDirection()
    {

        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (rb.velocity.x != 0 && (movementInputDirection < 0 || movementInputDirection > 0))
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void Flip()
    {
        if (canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
            if (isGrounded)
            {
                Dust.Play();
            }
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        if (isFacingRight)
        {
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));

        }
        else if (!isFacingRight)
        {
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x - wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        }
    }
}
