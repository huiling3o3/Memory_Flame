using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : IDataClass
{
    public string enemyId { get; private set; }
    public string enemyName { get; private set; }
    public int enemyHp { get; private set; }
    public int enemyAtk { get; private set; }
    public float enemyMoveSpeed { get; private set; }
    public int enemyAtkCooldown { get; private set; }

    public void SetData(params string[] input)
    {
        enemyId = input[0];
        enemyName = input[1];
        enemyHp = int.Parse(input[2]);
        enemyAtk = int.Parse(input[3]);
        enemyMoveSpeed = float.Parse(input[4]);
        enemyAtkCooldown = int.Parse(input[5]);
    }
}

