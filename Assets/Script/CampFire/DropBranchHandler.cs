using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBranchHandler : MonoBehaviour
{
    [SerializeField] private GameObject branchPrefab; // Assign the chest prefab in the inspector
    [SerializeField] protected int branchNum; //Number of branches to spawn

    protected virtual void DropBranches()
    {
        // Set a distance between each branch spawn
        float offsetDistance = 0.5f;

        for (int i = 0; i < branchNum; i++)
        {
            // Calculate the new position by adding an offset for each branch
            Vector3 newPosition = transform.position + new Vector3(0, i * offsetDistance, 0);

            // Instantiate the branch at the calculated position
            Instantiate(branchPrefab, newPosition, Quaternion.identity);
        }
    }
}
