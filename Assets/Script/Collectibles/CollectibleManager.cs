using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public int memoryFragmentsCollected = 0;
    public int sticksCollected = 0;

    // reference to the TextMeshPro UI elements
    public TextMeshProUGUI memoryFragmentsText;
    public TextMeshProUGUI sticksText;

    public void AddMemoryFragment()
    {
        memoryFragmentsCollected++;
        UpdateUI();
    }

    public void AddStick()
    {
        sticksCollected++;
        UpdateUI();
    }

    // update the TextMeshPro UI with the current count
    private void UpdateUI()
    {
        memoryFragmentsText.text = "Memory Fragments: " + memoryFragmentsCollected;
        sticksText.text = "Sticks: " + sticksCollected;
    }
}
