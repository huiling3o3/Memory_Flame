using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public int atkCooldown { get; set; }
    public Enemy(string id, string name, string desc, int hp, int atk, float moveSpeed, int atkCoolDown) : base(id, name, desc, UnitType.Enemy, hp, atk, moveSpeed) 
    {
        this.atkCooldown = atkCoolDown;
    }

}
