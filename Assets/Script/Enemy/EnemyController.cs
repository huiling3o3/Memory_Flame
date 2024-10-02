using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : DropBranchHandler
{
    //reference
    [SerializeField]
    private GameObject target; //player
    private Animator am;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    //Variables for enemy stats
    [Header("Enemies Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float maxHp;
    [SerializeField] public int atk;
    [SerializeField] private int atkCooldown;

    //Variables for movement
    private float distanceBtwPlayer;

    //Variables for attacks  
    private bool canAttack = true;
    private bool targetInRange = false;

    // Variables for color change effect
    [Header("Hit Settings")]    
    [SerializeField] private Color hitColor = Color.red; // Color when hit
    [SerializeField] private float colorChangeDuration = 0.1f; // Duration for the color change
    private Color originalColor; // Store the original color of the enemy

    [Header("Health Settings")]
    [SerializeField] private Image healthbar;
    [SerializeField] private float currentHp;

    private void Start()
    {
        //sprite render
        sr = GetComponent<SpriteRenderer>();

        //get enemy animator
        am = GetComponent<Animator>();

        // Get Rigidbody2D reference
        rb = GetComponent<Rigidbody2D>();

        originalColor = sr.color; // Save the original color of the enemy sprite

        //set currentHp to max hp
        currentHp = maxHp;
        UpdatHealthBar();
    }
    public void Init()
    {
        //Initializes the reference
        target = Game.GetPlayer().gameObject;

        //sprite render
        sr = GetComponent<SpriteRenderer>();

        originalColor = sr.color; // Save the original color of the enemy sprite
    }

    private void FixedUpdate()
    {
        if (canAttack)
        {
            // Move towards the player if allowed to attack
            GetTarget(target);
            //set animation to walk
            am.SetBool("isWalking", true);
            // Checks if target is within range for attacking
            if (targetInRange)
            {
                StartCoroutine(Attack());
            }
        }
        else
        {
            //set animation to idle
            am.SetBool("isWalking", false);
        }
    }

    //Initializes the enemy stats 
    public void SetStats(int hp, int atk, float speed, int atkCooldown)
    {
        Debug.Log("Set stats entered");

        this.maxHp = hp;
        currentHp = maxHp;
        this.atk = atk;
        this.atkCooldown = atkCooldown;
    }

    //Controls the enemy movement 
    public void GetTarget(GameObject target)
    {
        if (target == null)
        {
            return;
        }

        //get the distance between the player and enemy
        distanceBtwPlayer = Vector2.Distance(transform.position, target.transform.position);

        //get the direction of the player
        Vector2 direction = target.transform.position - transform.position;
        direction.Normalize();

        // Flip the sprite based on the direction of movement
        if (direction.x < 0)
        {
            sr.flipX = true; // Moving left, flip the sprite
        }
        else if (direction.x > 0)
        {
            sr.flipX = false; // Moving right, do not flip
        }
        //Debug.Log("Distance between player: " + distanceBtwPlayer.ToString());
        // Move the enemy using Rigidbody2D.MovePosition
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    //Trigger used to define enemy attack radius 
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetInRange = true;
        }         

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            targetInRange = false;
    }

    void stopMoving()
    {
        rb.velocity = Vector2.zero; // Stops the Rigidbody2D's movement
    }

    void Moving()
    { 
        rb.velocity *= speed;
    }
    IEnumerator Attack()
    {
        //Debug.Log("timer start");
        canAttack = false;
        stopMoving();
        // TODO: Play the enemy hit sound
        SoundManager.PlaySound(SoundType.CLAW_ATTACK);
        //attack animation
        am.SetTrigger("attack");
        Moving();
        //Timer to add in pauses between attacks
        yield return new WaitForSeconds(atkCooldown);
        canAttack = true;
    }

    public void TakeDamage(float damage)
    {        
        currentHp -= damage;

        // Start the color change effect
        StartCoroutine(OnHit());

        Debug.Log($"Enemy took {damage} damage");

        if (currentHp <= 0)
        {
            Destroy(gameObject);
            Game.GetGameController().EnemyKilled();
            DropBranches();
        }

        //update the UI
        UpdatHealthBar();
    }

    //update enemy health bar
    void UpdatHealthBar()
    {
        healthbar.fillAmount = currentHp / maxHp;
    }

    // change the color when hit
    IEnumerator OnHit()
    {
        // Change the sprite color to the hit color
        sr.color = hitColor;
        // TODO: Play the enemy hurt sound

        // Stop the enemy movement for a while
        canAttack = false;
        // Wait for the duration of the color change
        yield return new WaitForSeconds(colorChangeDuration);
        // resume the enemy movement
        canAttack = true;
        // Revert the sprite color back to the original color
        sr.color = originalColor;
    }
}
