using System;
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
    [SerializeField] float currentColdLvl;
    [SerializeField] float maxColdLvl = 100f;// Maximum cold bar
    [SerializeField] float coldRate = 10f; // Coldness increases per second when out of warm zone, decrease it to make it slower
    [SerializeField] float coldDamagePower = 10f; // The amount of damage to decrease the health if hit the max lvl

    //[Header("UI Elements")]
    //[SerializeField] StatusBar hpBar;
    //[SerializeField] StatusBar hypoBar;

    //references
    PlayerMovement pm;
    Animator am;

    void Awake()
    {
        am = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
    }

    //called by the game controller when the game starts
    public void Init()
    {
        //set player initial position
        transform.position = Vector2.zero;
        //reset all the variables
        Reset();
        //set all the references connected to the player interactions
        //pm = GetComponent<PlayerMovement>();
        //am = GetComponent<Animator>();
    }

    private void Update()
    {       
        if (!playerDead)
        {
            // increase coldness over time
            IncreaseColdness();

            // Update the player's health and cold UI
            Game.GetHUDController().UpdateHealthBar(currentHp, MaxHP);
            Game.GetHUDController().UpdateColdBar(currentColdLvl, maxColdLvl);

            //Check player movement and change sprite
            if (pm.moveDir.x != 0 || pm.moveDir.y != 0)
            {
                am.SetBool("Move", true);
            }
            else
            {
                am.SetBool("Move", false);
            }
        }     
    }

    //Rest function
    public void Reset()
    {
        currentHp = MaxHP;
        currentColdLvl = 0;
        inSafeZone = false;
        playerDead = false;
    }

    public float GetMovementSpeed() => pm.moveSpeed;
    public float GetMaxHp() => MaxHP;
    public float GetCurrentHp() => currentHp;
    public Vector2 GetLastMovedVector() => pm.lastMovedVector;
    public Vector2 GetMoveDir() => pm.moveDir;
    public void IncreaseHealth(float newHp) //newHp is in percentage
    {
        currentHp += currentHp * newHp;
        currentHp = Mathf.Clamp(currentHp, 0, MaxHP); // Ensure health doesn't go above 0
    }

    public void TakeDamage(float damage)
    {
        if (currentHp > 0.1)
        {
            currentHp -= damage;
            currentHp = Mathf.Clamp(currentHp, 0, MaxHP); // Ensure health doesn't go below 0
            Debug.Log($"player took {damage} damage");
        }
        
        if(currentHp <= 0.1)
        {
            Debug.Log("Character dead");
            playerDead = true;
            Game.GetGameController().GameOver();
        }
    }

    //Hypothermia System
    private void IncreaseColdness()
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

    // Update both the health and cold bars
    //private void UpdateUI()
    //{
    //    hpBar.SetState(currentHp, MaxHP);
    //    hypoBar.SetState(currentColdLvl, maxColdLvl);
    //}

    public void ExitSafeZone() { inSafeZone = false; }
    public void EnterSafeZone()
    {
        //Call this method when player is back into campfire 
        if (playerDead) return;
        currentColdLvl = 0;
        inSafeZone = true;
    }
    public bool IsPlayerInSafeZone() => inSafeZone;
    // function to check the current coldness for UI purposes
    public float GetCurrentColdLevel()
    {
        return currentColdLvl;
    }
}
