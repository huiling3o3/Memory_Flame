using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : DropBranchHandler
{
    //reference
    private GameObject target; //player
    private Animator am;
    private SpriteRenderer sr;

    //Holds navmesh agent reference 
    private NavMeshAgent agent;

    //Variables for enemy stats
    [Header("Enemies Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float maxHp;
    [SerializeField] public int atk;
    [SerializeField] private int atkCooldown;
    [SerializeField] private float attackDistanceThreshold = 0.8f;
    [SerializeField] private float chaseDistanceThreshold = 3f;

    //Variables for movement
    [SerializeField] private float distanceBtwPlayer;
    [SerializeField] private Transform initialPosition;
    private bool isFacingRight = true;
    private bool haveTarget = false;
    //Variables for attacks  
    private bool canAttack = true;

    // Variables for color change effect
    [Header("Hit Settings")]    
    [SerializeField] private Color hitColor = Color.red; // Color when hit
    [SerializeField] private float colorChangeDuration = 0.1f; // Duration for the color change
    private Color originalColor; // Store the original color of the enemy

    [Header("Health Settings")]
    [SerializeField] private Image healthbar;
    [SerializeField] private float currentHp;

    void Awake()
    {
        //get references
        sr = GetComponent<SpriteRenderer>();
        //get enemy animator
        am = GetComponent<Animator>();

        //nav mesh agent
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;

        //store their original position
        initialPosition = this.transform;
        // Save the original color of the enemy sprite
        originalColor = sr.color;
    }

    public void Init(GameObject Target)
    {
        //reset original posiion
        this.transform.position = initialPosition.position;

        //reset enemies stats
        canAttack = true;
        isFacingRight = true;
        haveTarget = false; //Set have target to false, so it will only attack the player when it is near.

        sr.color = originalColor; // reset the sprite color

        //set currentHp to max hp
        currentHp = maxHp;
        UpdatHealthBar();

        target = Target;
    }
    private void OnEnable()
    {
        // Subscribe to the events
        GameController.OnGamePaused += HandleGamePaused;
        //GameController.OnGameResumed += HandleGameResumed;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        GameController.OnGamePaused -= HandleGamePaused;
        //GameController.OnGameResumed -= HandleGameResumed;
    }
    private void FixedUpdate()
    {
        if (target == null || Game.GetGameController().isGameOver)
        {
            //stop enemy from moving
            stopMoving();
            return;
        }

        //get the distance between the player and enemy
        distanceBtwPlayer = Vector2.Distance(target.transform.position, transform.position);

        //Check if target is within range to chase
        if (distanceBtwPlayer < chaseDistanceThreshold)
        {
            haveTarget = true;
        }

        if (haveTarget && canAttack)
        {
            // Checks if target is within range for attacking
            if (distanceBtwPlayer < attackDistanceThreshold)
            {
                Attack(target.gameObject);
            }
            else
            {
                // Move towards the player
                ChaseTarget(target);
            }
        }
    }

    // Called when the game is paused
    private void HandleGamePaused(bool isPaused)
    {
        if (isPaused)
        {
            //Debug.Log($"{gameObject.name} received pause notification");
            stopMoving();
        }
        else
        {
            //Debug.Log($"{gameObject.name} received resume notification");
            agent.isStopped = false;
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

    public void ChaseTarget(GameObject target)
    {
        //chase the player
        agent.SetDestination(new Vector3(target.gameObject.transform.position.x, target.gameObject.transform.position.y, 0f));
        
        //get the direction of the player
        Vector2 direction = target.transform.position - transform.position;
        direction.Normalize();

        // Flip the sprite based on the direction of movement
        if (direction.x < 0)
        {
            FlipRight(false);
        }
        else if (direction.x > 0)
        {
            FlipRight(true);
        }

        // Move the enemy using Rigidbody2D.MovePosition
        //rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        //set animation to walk
        am.SetBool("isWalking", true);
    }

    private void FlipRight(bool faceRight)
    {
        isFacingRight = faceRight;

        //change the player's X value to flip
        Vector3 localScale = transform.localScale;
        localScale.x = faceRight ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    void Attack(GameObject objToDamage)
    {
        Debug.Log("Attacking target");
        //attack animation
        am.SetTrigger("Attacking");

        //damage the player
        PlayerController playerHealth = objToDamage.GetComponent<PlayerController>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(atk);
        }

        //Play the enemy hit sound
        SoundManager.PlaySound(SoundType.CLAW_ATTACK);
        
        //add the delay
        StartCoroutine(AttackTimer());
    }

    //Trigger used to define enemy attack radius 
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //targetInRange = true;
        }         

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //targetInRange = false;
        }
    }

    void stopMoving()
    {
        agent.isStopped = true;

        //set animation to walk
        am.SetBool("isWalking", false);
    }

    //Timer to add in pauses between attacks
    IEnumerator AttackTimer()
    {
        Debug.Log("timer start");
        canAttack = false;
        stopMoving();
        //Timer to add in pauses between attacks
        yield return new WaitForSeconds(atkCooldown);        
        canAttack = true;
        agent.isStopped = false;
    }

    public void TakeDamage(float damage)
    {        
        currentHp -= damage;

        // Start the color change effect
        StartCoroutine(OnHit());

        Debug.Log($"Enemy took {damage} damage");

        if (currentHp <= 0)
        {
            //destroy game object
            this.gameObject.SetActive(false);
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

        // Wait for the duration of the color change
        yield return new WaitForSeconds(colorChangeDuration);

        // Revert the sprite color back to the original color
        sr.color = originalColor;
    }
}
