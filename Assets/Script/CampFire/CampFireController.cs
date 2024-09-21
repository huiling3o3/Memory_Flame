using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFireController : MonoBehaviour
{
    // Fire System
    [Header("Fire System")]
    [SerializeField] private float maxHealth = 100f; 
    [SerializeField] private float currentHealth;
    [SerializeField] private float healthDepletionRate = 0.5f;

    // List of fire sprites for different health levels
    [SerializeField] private List<Sprite> fireSprites; // List of fire sprites
    SpriteRenderer fireSpriteRenderer;

    void Awake()
    {
        fireSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Initialize health
        UpdateFireAppearance();    // Set initial fire appearance
    }

    // Update is called once per frame
    void Update()
    {
        BurnFire();        // Deplete fire health over time
        UpdateFireAppearance();    // Update fire sprite based on current health
    }

    private void BurnFire()
    {
        // Deplete fire health over time
        currentHealth -= healthDepletionRate * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health between 0 and max
        
        // If the fire health reaches 0, trigger game over
        if (currentHealth <= 0)
        {
            Debug.Log("Fire Burned Out!!!");
            // Trigger game over event
            //Game.GetGameController().GameOver();
        }

    }

    private void UpdateFireAppearance()
    {
        if (fireSprites.Count == 0) { return ; }

        // Calculate the current fire health percentage
        float healthPercentage = currentHealth / maxHealth;

        // Determine which sprite to display based on the health percentage
        int spriteIndex = Mathf.FloorToInt(healthPercentage * (fireSprites.Count - 1));

        // Clamp the index to ensure it's within the bounds of the list
        spriteIndex = Mathf.Clamp(spriteIndex, 0, fireSprites.Count - 1);

        // Update the sprite renderer with the selected sprite
        fireSpriteRenderer.sprite = fireSprites[spriteIndex];
    }

    public void AddBranches(float branchAmount)
    {
        // Increase the fire health when branches are added
        currentHealth += branchAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health to max value
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerShoot ps = collision.GetComponent<PlayerShoot>();
            ps.RegenerateAmmo();
            PlayerController pc = collision.GetComponent<PlayerController>();
            pc.EnterSafeZone();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            pc.ExitSafeZone();
        }
    }

}
