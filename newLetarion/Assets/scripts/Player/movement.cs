using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class movement : MonoBehaviour
{
    public GameMaster gm;

    public float maxDistance = 50f;
    public bool HasTouchedGround;

    public float movementInputDirection;
    public float movementInputDirectionY;

    private float jumpTimer;

    private float DashTimeLeft;

    private float lastImageXPos;
    private float lastImageYPos;
    private float lastDash = -100f;


    private Rigidbody2D rb;
    private Animator anim;

    public int MaxHealth = 10;
    public int currentHealth;

    public float facingDirection;
    public float movementSpeed = 10;
    public float Jumpforce = 21.0f;
    public float groundCheckRadius = 0.35f;
    public float jumpHightMultiplier = 0.5f;
    public float jumpTimerSet = 0.20f;
    public float wallCheckDistance;

    public GameObject Skull;

    public float dashTime;

    public float DashSpeed;
    public float DashSpeedY;
    public float distanceBetweenImages;
    public float DashCoolDown;


    public int DashAmmount = 2;
    public int DashCount;

    public Transform groundCheck;
    public Transform wallCheck;

    public bool isWalking;
    public bool isFacingRight = true;
    public bool isGrounded;
    public bool canJump;
    public bool isAttemptingToJump;
    public bool isTouchingWall;
    public bool isDashing;
    private bool canMove;
    private bool canFlip;
    private bool CheckJumpMultiplier;

    public LayerMask whatIsGround;

    public ParticleSystem Dust;
    public ParticleSystem Death;

    public owlManager OM;
    public DialogManager DM;

    public SlowMotion slowMotion;

    public float GameOverTime;
    public float deathParticalTime;

    public float diagonalDashSpeed;
    public bool whasGrounded;
    private float leaveGroundTimer;
    public float leaveGroundTimerSet;

    public float dashEndMultiplier;

    void Start()
    {
        facingDirection = 1;
        Skull.SetActive(false);
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.LastCheckPointPos;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = MaxHealth;
        DashCount = DashAmmount;
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
        StartCoroutine(CheckDash());
        StartCoroutine(CheckHealth());
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
        //Vector3 mousePos = (Input.mousePosition);
        
    }

    IEnumerator CheckHealth()
    {
        if (currentHealth <= 0)
        {

            rb.velocity = new Vector2(0, 0);
            Death.Play();
            yield return new WaitForSeconds(deathParticalTime);
            Skull.SetActive(true);
            yield return new WaitForSeconds(GameOverTime);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        movementInputDirectionY = Input.GetAxisRaw("Vertical");

        if (isGrounded && Time.time >= lastDash + DashCoolDown)
        {
            DashCount = DashAmmount;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (whasGrounded))
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

        if (Input.GetButtonDown("Dash"))
        {
            if (DashCount > 1 && Time.time >= lastDash + DashCoolDown)
            {
                DashCount -= 1;
                AttemptToDash();
            }
            else if (DashCount == 1)
            {
                DashCount = DashAmmount;
                AttemptToDash();
            }
        }
    }


    private void ApplyMovement()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
    }

    private void CheckJump()
    {
        if(jumpTimer > 0)
        {
            if (isGrounded)
            {
                NormalJump();
            }
        }

        if(isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }


        if(isGrounded)
        {
            leaveGroundTimer = leaveGroundTimerSet;
        }
        else if(!isGrounded)
        {
            leaveGroundTimer -= Time.deltaTime;
        }
        
        if(!isGrounded && leaveGroundTimer <= 0)
        {
            whasGrounded = false;
        }
        else if(!isGrounded && leaveGroundTimer > 0)
        {
            whasGrounded = true;
        }
    }

    private void NormalJump()
    {
        if(canJump)
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
        if ((isGrounded && rb.velocity.y <= 0.01f) || whasGrounded)
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
        
        if (isFacingRight && movementInputDirection < 0 )
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

    private void AttemptToDash()
    {
        isDashing = true;
        DashTimeLeft = dashTime;
        lastDash = Time.time;
        
        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXPos = transform.position.x;
        lastImageYPos = transform.position.y;
    }

    IEnumerator CheckDash()
    {
        /// apply dash
        if (isDashing)
        {
            
            canMove = false;
            canFlip = false;

            if(movementInputDirection != 0 && movementInputDirectionY != 0)
            {
                rb.velocity = new Vector2(movementInputDirection * DashSpeed, movementInputDirectionY * DashSpeedY);
            }
            else if((movementInputDirection == 0 && movementInputDirectionY == 0) || (movementInputDirectionY == 0))
            {
                rb.velocity = new Vector2(facingDirection * DashSpeed, 0);
            }
            else if (movementInputDirection == 0 && (movementInputDirectionY == 1 || movementInputDirectionY == -1))
            {
                rb.velocity = new Vector2(0, movementInputDirectionY * DashSpeedY);                
                ///rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            
            DashTimeLeft -= Time.deltaTime;

            if (Mathf.Abs(transform.position.x - lastImageXPos) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageXPos = transform.position.x;
            }

            if (Mathf.Abs(transform.position.y - lastImageYPos) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageYPos = transform.position.y;
            }

            if(rb.velocity.y > 0.0f)
            {
                yield return new WaitForSeconds(dashTime);
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * dashEndMultiplier);
            }
        }

        ///Stop dashing
        if (DashTimeLeft <= 0 || isTouchingWall)
        {
            isDashing = false;
            canMove = true;
            canFlip = true;
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



