using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IInputReceiver
{
    //References
    private Rigidbody2D rb;
    PlayerController controller;
    //Movement
    public float moveSpeed;
    [SerializeField] private bool isFacingRight = true;
    //Dashing
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 20f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;

    [HideInInspector]
    public Vector2 moveDir { get; set; }
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 lastMovedVector;
    void Start()
    {
        //set up the rigidbody
        rb = GetComponent<Rigidbody2D>();
    
        //Store the last moved vector, so when the projectile weapon move it will not remain 0 
        lastMovedVector = new Vector2(1, 0f); 
        moveDir = Vector2.zero;
    }

    public void ChangeMovementSpeed(float newMoveSpeed)
    {
        if (moveSpeed != 0)
        {
            moveSpeed = newMoveSpeed;
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        if (lastMovedVector.x == 0 && lastMovedVector.y > 0) //top
        {
            rb.velocity = new Vector2(0f, transform.localScale.y * dashingPower);
        }
        else if (lastMovedVector.x == 0 && lastMovedVector.y < 0) //down
        {
            rb.velocity = new Vector2(0f, transform.localScale.y * dashingPower * -1f);
        }
        else if (lastMovedVector.x < 0 && lastMovedVector.y == 0) //left
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);           
        }
        else if (lastMovedVector.x > 0 && lastMovedVector.y == 0) //right
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        }

        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);

        // Stop dashing
        rb.velocity = Vector2.zero;
        isDashing = false;
        tr.emitting = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void FlipRight(bool faceRight)
    {
        isFacingRight = faceRight;

        //change the player's X value to flip
        Vector3 localScale = transform.localScale;
        localScale.x = faceRight? Mathf.Abs(localScale.x): -Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    public bool isPlayerFacingRight() { return isFacingRight; }

    #region Input handling
    public void DoDash()
    {
        if (isDashing)
        {
            return;
        }

        if (canDash)
        {
            StartCoroutine(Dash());
        }
        
    }

    public void DoMoveDir(Vector2 aDir)
    {
        if (isDashing)
        {
            // Don't allow normal movement during dash or knockback
            return;
        }

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontalVector, 0f);    //Last moved X
        }

        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(0f, lastVerticalVector);  //Last moved Y
        }

        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector);    //While moving
        }

        if (moveDir.x > 0)
        {
            FlipRight(true); //face right
        }
        else if (moveDir.x < 0)
        {
            FlipRight(false); //face left
        }

        //get the movement direction
        moveDir = aDir;
       
        moveDir = Vector2.ClampMagnitude(moveDir, 0.1f);

        //normalize it to prevent diagonal movement being faster
        moveDir = moveDir.normalized;

        // Move the player object using MovePosition function of Rigidbody2D
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }
    public void DoLeftAction()
    {
        //do nothing
    }

    public void DoRightAction()
    {
        //do nothing
    }

    public void DoSubmitAction()
    {
        //do nothing
    }

    public void DoCancelAction()
    {
        //pause game
        Game.GetGameController().OpenPauseMenu();
    }

    #endregion Input handling
}