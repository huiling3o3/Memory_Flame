using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    [Header("Player Stats")]

    [SerializeField] float currentHp;

    [SerializeField] float MaxHP;

    //[SerializeField] StatusBar hpBar;

    //references
    PlayerMovement pm;
    Animator am;
    SpriteRenderer sr;

    void Awake()
    {
        am = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Init()
    {
        //set player initial position
        transform.position = Vector2.zero;
        //set all the references connected to the player interactions
        //pm = GetComponent<PlayerMovement>();
        //am = GetComponent<Animator>();
    }

    private void Update()
    {
        //for debugging purpose only
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            
        }

        //Check player movement and change sprite
        if (pm.moveDir.x != 0 || pm.moveDir.y != 0)
        {
            am.SetBool("Move", true);

            //SpriteDirectionChecker();
        }
        else
        {
            am.SetBool("Move", false);
        }
    }
    

    //Adjust sprite animation function
    void SpriteDirectionChecker()
    {
        if (pm.lastHorizontalVector < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
    //Rest function
    public void Reset()
    {
        currentHp = 0;
        MaxHP = 0;
    }

    #region player movement function
    public float GetMovementSpeed() => pm.moveSpeed;
    public float GetMaxHp() => MaxHP;
    public float GetCurrentHp() => currentHp;
    public Vector2 GetLastMovedVector() => pm.lastMovedVector;
    public Vector2 GetMoveDir() => pm.moveDir;
    public void IncreaseHealth(float newHp) //newHp is in percentage
    {
        currentHp += currentHp * newHp;
        if (currentHp > MaxHP)
        { 
            currentHp = MaxHP;
        }

        //create an event subscription when player health is decreased
        //hpBar.SetState(currentHp, MaxHP);
    }

    public void TakeDamage(int damage)
    {
        if (currentHp >= 0)
        {
            currentHp -= damage;
        }
        Debug.Log($"player took {damage} damage");
        if (currentHp <= 0)
        {
            Debug.Log("Character dead");
            //Game.GetGameController().GameOver();
        }

        //create an event subscription when player health is decreased
        //hpBar.SetState(currentHp, MaxHP);
    }

    #endregion
}
