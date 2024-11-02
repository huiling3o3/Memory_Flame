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
    [SerializeField] private bool canDash = true;
    public bool isDashing;
    [SerializeField] private float dashingPower = 5f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

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

        // Use the last moved vector for dash direction
        Vector2 dashDirection = lastMovedVector.normalized;
        rb.velocity = dashDirection * dashingPower;

        // Enable dash trail effect
        tr.emitting = true;

        // Dash duration
        yield return new WaitForSeconds(dashingTime);

        // Stop dashing
        rb.velocity = Vector2.zero;
        isDashing = false;
        tr.emitting = false;

        // Dash cooldown
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
        if (Game.GetGameController().isGameOver)
        {
            return;
        }
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

        if (Game.GetGameController().isGameOver || Game.GetGameController().isPaused)
        {
            return;
        }

        if (isDashing)
        {
            // Don't allow normal movement during dash
            return;
        }

        // Update last moved vector if there's movement input
        if (aDir != Vector2.zero)
        {
            lastMovedVector = aDir.normalized;  // Update to track the latest direction, including diagonals
        }

        // Flip the sprite based on horizontal movement
        if (aDir.x > 0)
        {
            FlipRight(true); // face right
        }
        else if (aDir.x < 0)
        {
            FlipRight(false); // face left
        }

        // Normalize and apply movement
        moveDir = aDir.normalized;
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
        if (!Game.GetGameController().isGameOver)
        {
            //pause or unpause
            Game.GetGameController().TogglePause();
        }
    }

    #endregion Input handling
}