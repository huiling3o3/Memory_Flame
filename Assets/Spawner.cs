using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject treePrefab;
    public float minTravelDistance = 2f;
    Vector2 boundaryMin, boundaryMax;

    [SerializeField]
    private List<GameObject> spawnedTree = new List<GameObject>(); //List to contain all spawned tree
    private int maxAttempts = 10;
    // Start is called before the first frame update
    void Start()
    {
        //set game boundary according to camera boundary (for fixed camera)
        Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        boundaryMin = mainCamera.ViewportToWorldPoint(new Vector2(0.1f, 0.1f));
        boundaryMax = mainCamera.ViewportToWorldPoint(new Vector2(0.9f, 0.9f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnTree(5);
        }
    }

    public void ClearSpawnedTree()
    {
        if (spawnedTree.Count != 0)
        {
            for (int i = 0; i < spawnedTree.Count; i++)
            {
                Destroy(spawnedTree[i]);
            }

            //clear list
            spawnedTree.Clear();
        }

    }

    // Randomize position while avoiding both the player and other spawned trees
    private Vector2? GetValidRandomPos(Vector2 playerPos)
    {
        Vector2 randomPos;
        bool isPositionValid = false;

        // Try to find a valid position within a limited number of attempts
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            // Generate a random position within game boundary
            randomPos = new Vector2(
                Random.Range(boundaryMin.x, boundaryMax.x),
                Random.Range(boundaryMin.y, boundaryMax.y)
            );

            // Check distance to the player
            if (Vector2.Distance(randomPos, playerPos) > minTravelDistance)
            {
                isPositionValid = true;

                // Check distance to all existing trees
                foreach (GameObject tree in spawnedTree)
                {
                    if (Vector2.Distance(randomPos, tree.transform.position) <= minTravelDistance)
                    {
                        isPositionValid = false;
                        break;
                    }
                }

                // If position is valid, return it
                if (isPositionValid)
                {
                    return randomPos;
                }
            }
        }

        // Return null if no valid position is found after max attempts
        return null;
    }

    public void SpawnTree(int amount)
    {
        Vector2 playerPos = Game.GetPlayer().transform.position;

        for (int i = 0; i < amount; i++)
        {
            // Try to find a valid position to spawn the tree
            Vector2? validPos = GetValidRandomPos(playerPos);

            // Only instantiate the tree if a valid position was found
            if (validPos.HasValue)
            {
                GameObject spawn = Instantiate(treePrefab);
                spawn.transform.position = validPos.Value; // Set the valid position
                spawnedTree.Add(spawn); // Add the tree to the list of spawned trees
            }
            else
            {
                Debug.Log("Could not find a valid position to spawn a tree.");
            }
        }
    }
}
