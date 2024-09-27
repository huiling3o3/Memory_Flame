using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    private List<GameObject> spawnedEnemies = new List<GameObject>(); //List to contain all enemies 

    private void Awake()
    {
        Game.SetEnemySpawner(this);
    }

    public void ClearSpawnedEnemies()
    {
        if (spawnedEnemies.Count != 0)
        {
            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                Destroy(spawnedEnemies[i]);
            }

            //clear list
            spawnedEnemies.Clear();
        }
    
    }

    //change the enemy name to enemyID when change to wave manager
    public void SpawnEnemy(string enemyID, Transform spawnLocation)
    { 
        GameObject spawn = null;

        Enemy enemy = Game.GetEnemyByRefID(enemyID);
        spawn = Instantiate(enemyPrefab);
        //Set the spawn position
        spawn.transform.position = spawnLocation.position;
        spawn.transform.parent = spawnLocation;
        //initialise the enemy stats and start its function
        spawn.GetComponent<EnemyController>().Init();
        spawn.GetComponent<EnemyController>().SetStats(enemy.enemyHp, enemy.enemyAtk, enemy.enemyMoveSpeed, enemy.enemyAtkCooldown);
        spawnedEnemies.Add(spawn); //Adds to the list of enemies 
    }
}
