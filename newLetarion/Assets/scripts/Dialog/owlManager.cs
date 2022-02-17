using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class owlManager : MonoBehaviour
{
    public float PlayerCheckOwlRadius;

    public Transform PlayerCheck;

    public LayerMask whatIsRaven;

    public bool RavenInRange;

    public float PlayerSideDistence;
    public float PlayerSideDistenceL;


    public Transform playerSidePos;
    public Transform playerSidePosL;
    public Transform target;

    public float LookDirection;

    public bool RavenLocationL;
    public bool RavenLocationR;

    public DialogManager dialogManager;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DialogDirection();
    }

    private void FixedUpdate()
    {
        if(dialogManager.DialogEnded == false)
        {
            CheckSurroundings();
        }
        else if (dialogManager.DialogEnded)
        {
            if(LookDirection == -1)
            {
                Turn();
            }

        }
    }

    void DialogDirection()
    {
        if (RavenLocationL && RavenLocationR)
        {
            transform.Rotate(0, 0, 0);
        }
        else if(RavenLocationL && LookDirection == 1)
        {
            Turn();
        }
        else if(LookDirection == -1 && RavenLocationR)
        {
            Turn();
        }
    }

    void Turn()
    {
        transform.Rotate(0, 180, 0);
        LookDirection *= -1;
    }

    private void CheckSurroundings()
    {
        RavenInRange = Physics2D.OverlapCircle(PlayerCheck.position, PlayerCheckOwlRadius, whatIsRaven);

        RavenLocationR = Physics2D.OverlapCircle(playerSidePos.position, PlayerSideDistence, whatIsRaven);
        RavenLocationL = Physics2D.OverlapCircle(playerSidePosL.position, PlayerSideDistenceL, whatIsRaven);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PlayerCheck.position, PlayerCheckOwlRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerSidePos.position, PlayerSideDistence);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(playerSidePosL.position, PlayerSideDistenceL);


    }
}
