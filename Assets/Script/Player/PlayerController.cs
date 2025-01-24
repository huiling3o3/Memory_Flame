using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This script controls the player's animation and the player health and Hypothermia
public class PlayerController : MonoBehaviour
{    
    [Header("Player Stats")]
    [SerializeField] float currentHp;
    [SerializeField] float MaxHP = 100f;
    [SerializeField] private bool inSafeZone; //bool to check not affect the ammoDepletion
    [SerializeField] private bool playerDead; //bool to check player dead to stop the coldlvl from increasing

    [Header("Hypothermia System")]
    float currentColdLvl;
    float maxColdLvl = 100f;// Maximum cold bar
    float coldRate = 2f; // Coldness increases per second when out of warm zone, decrease it to make it slower
    float regainWarmthRate = 10f;
    float coldDamagePower = 10f; // The amount of damage to decrease the health if hit the max lvl

    // Variables for color change effect
    [Header("Hit Settings")]
    [SerializeField] private Color hitColor = Color.red; // Color when hit by enemies
    [SerializeField] private Color hitFreezeColor = Color.blue; // Color when hit by enemies
    private float colorChangeDuration = 0.1f; // Duration for the color change
    private Color originalColor; // Store the original color of the enemy


    //references
    private AudioSource audioSource;
    private Animator am;
    public PlayerMovement pm;
    public PlayerShoot ps;  
    public SpriteRenderer sr;
    
    void Awake()
    {
        //sprite render
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        am = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        ps = GetComponent<PlayerShoot>();
        originalColor = sr.color; // Save the original color of the enemy sprite
        //Set the reference to Game
        Game.SetPlayer(this);
    }

    //called by the game controller when the game starts
    public void Start()
    {
        //reset all the variables
        Reset();
        sr.color = originalColor; // reset the sprite color       
    }

    private void Update()
    {
        if (!playerDead && !Game.GetGameController().isPaused && !Game.GetGameController().isGameOver)
        {
            // increase coldness over time
            IncreaseColdness();

            // Update the player's health and cold UI
            Game.GetHUDController().UpdateHealthBar(currentHp, MaxHP);
            Game.GetHUDController().UpdateColdBar(currentColdLvl, maxColdLvl);

            //Check player movement
            if (pm.moveDir.x != 0 || pm.moveDir.y != 0)
            {
                am.SetBool("Move", true);
            }
            else
            {
                am.SetBool("Move", false);
            }

        }
        else if (Game.GetGameController().isPaused || Game.GetGameController().isGameOver)
        {
            am.SetBool("Move", false);
        }
    }

    //Rest function
    public void Reset()
    {
        currentHp = MaxHP;
        currentColdLvl = 0;
        inSafeZone = false;
        playerDead = false;
        ps.Reset();
        am.Play("Idle");
    }

    public void TakeDamage(float damage)
    {       
        if(currentHp <= 0.1)
        {
            Debug.Log("Character dead");
            playerDead = true;
            Game.GetGameController().GameOver();
            //To Do: switch to dead animation
            am.SetTrigger("PlayerDead");
        }

        if (currentHp > 0.1)
        {
            // Start the color change effect
            StartCoroutine(OnHit(false));
            currentHp -= damage;
            currentHp = Mathf.Clamp(currentHp, 0, MaxHP); // Ensure health doesn't go below 0
            //Debug.Log($"player took {damage} damage");
        }
    }

    //Hypothermia System
    public void IncreaseColdness()
    {
        //check if player is not near the fire place then increase the coldness
        if (!inSafeZone)
        {
            // increase coldness over time
            currentColdLvl += coldRate * Time.deltaTime;
            currentColdLvl = Mathf.Clamp(currentColdLvl, 0, maxColdLvl); // Ensure coldness doesn't go beyond the max lvl
            // damage the health once the current lvl have reached its peak
            if (currentColdLvl >= maxColdLvl)
            {
                // Deplete the health over time
                TakeDamage(coldDamagePower * Time.deltaTime);
            }
        }
    }

    public void IncreaseColdnessRate()
    {
        coldRate += 0.2f;
    }

    public void TakeFreezeDamage(float damage)
    {
        // Start the color change effect
        StartCoroutine(OnHit(true));
        currentColdLvl += damage;
    }

    public void RegenerateWarmth()
    {
        //increase ammo over time
        currentColdLvl -= regainWarmthRate * Time.deltaTime;
        currentColdLvl = Mathf.Clamp(currentColdLvl, 0, maxColdLvl); // Ensure it doesn't go beyond max
    }

    public void PlayFootstepSound()
    {
        //SoundManager.PlaySound(SoundType.FOOTSTEP, audioSource);
    }

    public void ExitSafeZone() { inSafeZone = false; }
    public void EnterSafeZone()
    {
        //Call this method when player is back into campfire 
        if (playerDead) return;
        inSafeZone = true;
    }

    public bool IsWarmed()
    {
        return inSafeZone && currentColdLvl <= 10f;
    }

    public bool IsPlayerInSafeZone() => inSafeZone;

    // change the color when hit
    IEnumerator OnHit(bool freeze)
    {
        if (freeze)
        {
            // Change the sprite color to the hit color
            sr.color = hitFreezeColor;
        }
        else
            sr.color = hitColor;
        // TODO: Play the hurt sound
        //SoundManager.PlaySound(SoundType.HURT, audioSource, 0.5f);
        // Wait for the duration of the color change
        yield return new WaitForSeconds(colorChangeDuration);

        // Revert the sprite color back to the original color
        sr.color = originalColor;
    }
}
