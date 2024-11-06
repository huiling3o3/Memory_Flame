using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampFireController : MonoBehaviour
{
    // Fire System
    [Header("Fire System")]
    [SerializeField] private float maxHealth = 100f; 
    [SerializeField] private float currentHealth;
    [SerializeField] private float healthDepletionRate = 0.5f;
    [SerializeField] private int branchHealAmount = 2;
    [SerializeField] private int currentBranches = 0;
    [SerializeField] private int amountToReviveFire = 10;
    [SerializeField] private FireState InitialState;

    
    // List of fire sprites for different health levels
    [Header("To Assign")]
    [SerializeField] private List<Sprite> fireSprites; // List of fire sprites
    [SerializeField] SpriteRenderer fireSpriteRenderer;
    [SerializeField] GameObject instructions;
    [SerializeField] TextMeshProUGUI branchTxt;
    [SerializeField] private Slider fireHealthBar;
    //A int to check if the campfire have been refueled since its initial stage
    public int refilledCount = 0;
    private bool playerInRange = false;
    private Level_Controller lvlController;
    private PlayerController pc;
    private enum FireState
    {
        Burning, 
        Extinguished
    }

    [SerializeField]
    private FireState currentFireState = FireState.Burning;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize();
        pc = Game.GetPlayer();
    }

    public void Initialize(Level_Controller level_Controller)
    {       
        //Add the lvl controller ref
        lvlController = level_Controller;
        // Initialize health
        switch (InitialState)
        {
            case FireState.Burning:
                currentHealth = maxHealth;
                currentBranches = 0;
                currentFireState = FireState.Burning;
                break;
            case FireState.Extinguished:
                currentHealth = 0;
                currentBranches = 0;
                currentFireState = FireState.Extinguished;
                break;
        }
        // Set initial fire appearance
        UpdateFireAppearance();
        //set the interaction apperance to false
        instructions.SetActive(false);
        //set tutorial stats
        refilledCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (lvlController == null || Game.GetGameController().isPaused || Game.GetGameController().isGameOver)
        {
            return;
        }

        BurnFire(); // Deplete fire health over time
        UpdateFireAppearance(); // Update fire sprite based on current health
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            AddBranchesToFire(); // F key to add the branches to the campfire
        }
        if (playerInRange && currentFireState == FireState.Burning)
        {
            pc.RegenerateWarmth();
            pc.ps.RegenerateAmmo(); //regenrate the player shooting ammo
        }
    }

    private void BurnFire()
    {
        switch (currentFireState)
        {
            case FireState.Burning:
                // Deplete fire health over time
                currentHealth -= healthDepletionRate * Time.deltaTime;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health between 0 and max

                if (currentHealth <= 0)
                {
                    KillFire();
                }

                break;
            case FireState.Extinguished:
                //do nothing
                break;
        }
       
    }
    private void BorrowFire(int amount)
    {
        //only borrow fire if the player have firetorch
        if (!Game.GetGameController().HaveFireTorch())
        {
            return;
        }

        // take away the fire health used to replenish the fire torch
        currentHealth -= maxHealth * amount / 100f;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health between 0 and max

        // If the fire health reaches 0, trigger game over
        if (!lvlController.CheckGameOver() && currentHealth <= 0)
        {
            KillFire();            
        }
    }

    private void UpdateFireAppearance()
    {
        if (fireSprites.Count == 0) { return; }

        // Calculate the current fire health percentage
        float healthPercentage = currentHealth / maxHealth;

        fireHealthBar.value = healthPercentage;

        //display the UI
        switch (currentFireState)
        {
            case FireState.Burning:               
                //diplay the heath bar
                fireHealthBar.gameObject.SetActive(true);

                //hide the branch txt to revive
                branchTxt.gameObject.SetActive(false);               
                break;

            case FireState.Extinguished:
                //hide the heath bar
                fireHealthBar.gameObject.SetActive(false);

                //show the branch txt to revive
                branchTxt.gameObject.SetActive(true);

                //update the txt of the branch count
                branchTxt.text = $"{currentBranches} / {amountToReviveFire}";
                break;

        }

        // Update the apperance of the fire

        // Determine which sprite to display based on the health percentage
        int spriteIndex = Mathf.FloorToInt(healthPercentage * (fireSprites.Count - 1));

        // Adjust instructions position based on health percentage
        float yOffset = Mathf.Lerp(42.17f, -28.16f, 1 - healthPercentage); // Start high, go low as health decreases
        instructions.transform.localPosition = new Vector3(instructions.transform.localPosition.x, yOffset, instructions.transform.localPosition.z);
        // Clamp the index to ensure it's within the bounds of the list
        spriteIndex = Mathf.Clamp(spriteIndex, 0, fireSprites.Count - 1);

        // Update the sprite renderer with the selected sprite
        fireSpriteRenderer.sprite = fireSprites[spriteIndex];
    }

    public void AddBranches(int branchAmount)
    {
        // Increase the fire health based on the amount of branch the player had collected
        // Each branch is worth 2 points
        float healthIncreaseAmount = branchAmount * branchHealAmount;
        currentHealth += healthIncreaseAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health to max value
        Game.GetGameController().RemoveStick(branchAmount);
    }


    void AddBranchesToFire()
    {
        //Check in the game controller whether the player have enough branch
        int sticks = Game.GetGameController().GetSticks();

        switch (currentFireState)
        {
            case FireState.Burning:
                    
                if (sticks > 0)
                {
                    int sticksUsed = (int)((maxHealth - currentHealth) / branchHealAmount) + 1;
                    if (sticks >= sticksUsed)
                    {
                        AddBranches(sticksUsed);// if enough update the branches num in game controller & increase the branch health 
                    }
                    else
                    {
                        AddBranches(sticks);
                    }
                    //tutorial
                    ++refilledCount;
                    Debug.Log(name + "got healed");
                }
                else
                {
                    Debug.Log($"not enough branches");
                }
                break;

            case FireState.Extinguished:
                if (currentBranches != amountToReviveFire)
                {
                    int currentStickRequired = amountToReviveFire - currentBranches;
                    //check if player have enough sticks
                    if (sticks > 0 && sticks < currentStickRequired) //not enough stick
                    {
                        //take all the player have and store it into the current branches
                        currentBranches += sticks;
                        //remove player current stick amt
                        Game.GetGameController().RemoveStick(sticks);
                    }
                    else if (sticks >= currentStickRequired) //enough stick
                    {
                        //remove the amt of stick from player
                        Game.GetGameController().RemoveStick(currentStickRequired);
                        //revive the fire
                        currentFireState = FireState.Burning;
                        //set the current heath to max
                        currentHealth = maxHealth;
                        //update fire UI
                        UpdateFireAppearance();
                        //reset the current branches to revive
                        currentBranches = 0;
                        //tutorial
                        ++refilledCount;
                    }
                }               

                break;
        }
    }

    public bool IsRefilledOnce()
    {
        if (refilledCount == 1)
        {
            return true;
        }
        return false;
    }

    public bool IsRefilledAgain()
    {
        if (refilledCount > 1)
        {
            return true;
        }
        return false;
    }

    private void KillFire()
    {
        currentFireState = FireState.Extinguished;
        //Debug.Log("Fire Burned Out!!!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {                    
            //PlayerController pc = collision.GetComponent<PlayerController>();
            if (currentFireState == FireState.Burning)
            {
                pc.EnterSafeZone();
            }

            playerInRange = true;

            instructions.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = false;
            //PlayerController pc = collision.GetComponent<PlayerController>();
            if (currentFireState == FireState.Burning)
            {
                //Everytime the fire torch is regenerated, 2% of the fire is taken away
                BorrowFire(2);               
                pc.ExitSafeZone();               
            }
            instructions.SetActive(false);
        }
    }

}
