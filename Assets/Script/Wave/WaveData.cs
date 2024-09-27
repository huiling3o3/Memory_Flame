using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveData
{
    public string waveID { get; }
    public int waveNumber { get; }
    public string enemyID { get; }
    public int enemyCount { get; }
    public string barrelID { get; }
    public int barrelCount { get; }


    public WaveData(string waveID, int waveNumber, string enemyID, int enemyCount, string barrelID, int barrelCount)
    {
        this.waveID = waveID;
        this.waveNumber = waveNumber;
        this.enemyID = enemyID;
        this.enemyCount = enemyCount;
        this.barrelID = barrelID;
        this.barrelCount = barrelCount;
    }

}
