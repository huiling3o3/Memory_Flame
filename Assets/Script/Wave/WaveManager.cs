using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public EnemySpawner enemySpawner; // Reference to the EnemySpawner script
    //public BarrelSpawner barrelSpawner; // Reference to the BarrelSpawner script
    public Transform[] spawnLocations; // Array of spawn locations for the enemy

    private List<WaveData> waveDataList; // List to store wave data
    private int currentWave = 0; // Current wave number
    private int EnemyCountInWave = 0; // Total number of enemy per wave
    private string currentWaveID = string.Empty;   
    private Queue<WaveData> waveDataListQueue = new Queue<WaveData>(); //Adds waves into a queue to spawn 

    private float enemySpawnDelay = 5f;
    //private WaitForSeconds barrelSpawnDelay = new WaitForSeconds(10f);

    private void Awake()
    {
        Game.SetWaveManager(this);      
    }

    public void Start()
    {
        InitializeWaves();
    }

    private void Update()
    {
        //debugging purpose
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            //NextWave();
        }
    }

    private void InitializeWaves()
    {
        waveDataList = Game.GetWaveDataList(); // Get wave data from Game
        if(waveDataList != null)
        {
            Debug.Log("Added waves to list"); 
        }
        else
        {
            Debug.Log("Unable to find wave");
        }
    }

    public void NextWave()
    {        
        currentWave++;
        Debug.Log($"WaveManager: Starting wave {currentWave}");

        //Reset the enemies total count
        EnemyCountInWave = 0;

        // Add waves to the queue that match the current wave number
        for (int i = 0; i < waveDataList.Count; i++)
        {
            WaveData wd = waveDataList[i];
            if (currentWave == wd.waveNumber)
            {
                Debug.Log($"Enqueueing wave {wd.waveNumber}");                
                waveDataListQueue.Enqueue(wd);
                //Get the number of enemy in a wave
                EnemyCountInWave += wd.enemyCount;
            }
        }

        Debug.Log($"Total number of enemies in wave {currentWave}: {EnemyCountInWave}");

        // Start spawning enemies if there are waves in the queue
        if (waveDataListQueue.Count > 0)
        {
            Debug.Log("Starting to spawn enemies");
            StartCoroutine(SpawnEnemies());
        }
        else
        {
            Debug.Log("No waves to spawn");
            Game.GetGameController().GameOver();
        }
    }

    private IEnumerator SpawnEnemies()
    {
        Debug.Log("Spawning enemies");

        WaveData waveToSpawn = waveDataListQueue.Dequeue(); //Gets the wave to spawn 
        currentWaveID = waveToSpawn.waveId;
        Debug.Log($"WaveNo: {currentWave} Number of enemies: {waveToSpawn.enemyCount}");

        if (currentWave > 1)
        {
            //decrease the spawn delay for the next wave to increase the difficulty
            enemySpawnDelay -= 0.5f;
            Debug.Log($"Enemy Spawn Delay is {enemySpawnDelay}");
        }

        //Spawn barrel using the same wave data
        //StartCoroutine(SpawnBarrel(waveToSpawn));

        //Spawns enemies based on wave data
        for (int i = 0; i < waveToSpawn.enemyCount; i++)
        {
            Game.GetEnemySpawner().SpawnEnemy(waveToSpawn.enemyId, GetRandomSpawnLocation());
            yield return new WaitForSeconds(enemySpawnDelay);
        }       

        if (waveDataListQueue.Count != 0) //Check if queue has waves to spawn
        {
            StartCoroutine(SpawnEnemies()); //Repeat the function 
        }
    }

    public void WaveReset()
    {
        currentWave = 0;
        enemySpawner.ClearSpawnedEnemies();
        StopAllCoroutines();
    }

    public int GetEnemyCountInWave()
    {
        return EnemyCountInWave;
    }

    public int GetCurrentWave()
    { 
        return currentWave;
    }

    public string GetCurrentWaveID() { return currentWaveID; }

    // Gets a random spawn location from the array
    private Transform GetRandomSpawnLocation()
    {
        int index = Random.Range(0, spawnLocations.Length);
        return spawnLocations[index];
    }
}
