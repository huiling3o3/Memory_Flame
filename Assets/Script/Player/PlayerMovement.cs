using System.Collections;
using UnityEngine;
//This script controls the player's movement controls only
public class PlayerMovement : MonoBehaviour
{
    //References
    private Rigidbody2D rb;
    private PlayerController pc;
    //Movement
    public float moveSpeed = 4f;
    [SerializeField] private bool isFacingRight = true;

    public Vector2 moveDir { get; set; }

    private float lastHorizontalVector;

    private float lastVerticalVector;

    private Vector2 lastMovedVector;

    void Start()
    {
        //set up the rigidbody
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerController>();
        //Store the last moved vector, so when the projectile weapon move it will not remain 0 
        lastMovedVector = new Vector2(1, 0f);
        //reset the movement direction
        moveDir = Vector2.zero;
    }

    void Update()
    {

    }
    void FixedUpdate()
    {
        float horiInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        moveDir = new Vector2(horiInput, vertInput);
        DoMoveDir(moveDir);
    }
    public void ChangeMovementSpeed(float newMoveSpeed)
    {
        if (moveSpeed != 0)
        {
            moveSpeed = newMoveSpeed;
        }
    }

    public bool isPlayerFacingRight() { return isFacingRight; }

    public void DoMoveDir(Vector2 aDir)
    {
        if (Game.GetGameController().isGameOver || Game.GetGameController().isPaused)
        {
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
            pc.sr.flipX = false;
        }
        else if (aDir.x < 0)
        {
            pc.sr.flipX = true;
        }

        // Normalize and apply movement
        moveDir = aDir.normalized;
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }
}