using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //Gets a reference to the player 
    [SerializeField]
    private GameObject target;

    //References
    Animator am;
    SpriteRenderer sr;
    PlayerMovement pm;

    //Variables for enemy stats
    [Header("Enemies Stats")]
    [SerializeField]    private float speed;
    [SerializeField]    private float hp;
    [SerializeField]    private int atk;
    [SerializeField]    private int atkCooldown;
    [SerializeField]    private GameObject branchPrefab; // Assign the chest prefab in the inspector

    //Holds navmesh agent reference 
    private NavMeshAgent agent;

    //Variables for attacks  
    private bool canAttack = true;

    private bool targetInRange = false;

    public void Init()
    {
        //Initializes the reference
        target = Game.GetPlayer().gameObject;

        //sprite render
        sr = GetComponent<SpriteRenderer>();

        //player movement
        pm = target.GetComponent<PlayerMovement>();

        //nav mesh agent
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(canAttack)
        {
            //Calls the GetTarget function repeatedly to move the navmesh in the target's direction 
            GetTarget(target.gameObject);
            SpriteDirectionChecker();

            //Checks if target is within range 
            if (targetInRange)
            {
                //If target is within range, attack the target
                Attack(target.gameObject);
            }
        }
    }

    void SpriteDirectionChecker()
    {
        if (agent.velocity.x < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    //Initializes the enemy stats 
    public void SetStats(int hp, int atk, float speed, int atkCooldown)
    {
        Debug.Log("Set stats entered");

        this.hp = hp;
        this.atk = atk;
        agent.speed = speed;
        this.atkCooldown = atkCooldown;
    }

    //Controls the enemy movement 
    public void GetTarget(GameObject target)
    {
        agent.SetDestination(new Vector3(target.gameObject.transform.position.x, target.gameObject.transform.position.y, 0f)); 
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
        agent.isStopped = true;
        yield return new WaitForSeconds(atkCooldown);
        canAttack = true;
        agent.isStopped = false;
    }

    public void TakeDamage(float damage)
    {        
        hp -= damage;
        Debug.Log($"Enemy took {damage} damage");
        if (hp <= 0)
        {
            Destroy(gameObject);
            //Game.GetGameController().EnemyKilled();
            DropBranches();
        }
    }

    void DropBranches()
    {
        Instantiate(branchPrefab, transform.position, Quaternion.identity);
    }
}
