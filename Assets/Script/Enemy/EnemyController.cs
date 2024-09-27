using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //Gets a reference to the player 
    [SerializeField]
    private GameObject target;

    //References
    [SerializeField] Animator am;
    private SpriteRenderer sr;

    //Variables for enemy stats
    [Header("Enemies Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float maxHp;
    [SerializeField] private int atk;
    [SerializeField] private int atkCooldown;

    [Header("Reward Settings")]
    [SerializeField] private GameObject branchPrefab; // Assign the chest prefab in the inspector
    [SerializeField] private int branchNum; //Number of branches to spawn

    //Variables for movement
    private float distanceBtwPlayer;
    Vector2 direction;

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
        //am = GetComponent<Animator>();

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

    private void Update()
    {
        if (canAttack)
        {
            // Move towards the player if allowed to attack
            GetTarget(target);
            // Checks if target is within range for attacking
            if (targetInRange)
            {
                Attack(target);
            }
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

        //set enemy to follow the player using its position
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    //Trigger used to define enemy attack radius 
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            targetInRange = true; 
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            targetInRange = false;
    }

    void Attack(GameObject objToDamage)
    {

        PlayerController playerHealth = objToDamage.GetComponent<PlayerController>();
        PlayerMovement playerMovement = objToDamage.GetComponent<PlayerMovement>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(atk);
        }

        //Add in attack animations 

        Debug.Log("Attacking target");
        StartCoroutine(AttackTimer());
    }

    //Timer to add in pauses between attacks
    IEnumerator AttackTimer()
    {
        // Debug.Log("Timer started");
        canAttack = false;
        //am.SetBool("isWalking", false);
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
            //Game.GetGameController().EnemyKilled();
            DropBranches();
        }

        UpdatHealthBar();
    }
    void UpdatHealthBar()
    {
        healthbar.fillAmount = currentHp / maxHp;
    }

    // change the color when hit
    IEnumerator OnHit()
    {
        // Change the sprite color to the hit color
        sr.color = hitColor;
        // Stop the enemy movement for a while
        canAttack = false;
        // Wait for the duration of the color change
        yield return new WaitForSeconds(colorChangeDuration);
        // resume the enemy movement
        canAttack = true;
        // Revert the sprite color back to the original color
        sr.color = originalColor;
    }

    void DropBranches()
    {
        // Set a distance between each branch spawn
        float offsetDistance = 0.5f;

        for (int i = 0; i < branchNum; i++)
        {
            // Calculate the new position by adding an offset for each branch
            Vector3 newPosition = transform.position + new Vector3(i * offsetDistance, 0, 0);

            // Instantiate the branch at the calculated position
            Instantiate(branchPrefab, newPosition, Quaternion.identity);
        }
    }
}
