using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveData: IDataClass
{
    public string waveId  { get; private set; }
    public int waveNumber { get; private set; }
    public string enemyId { get; private set; }
    public int enemyCount { get; private set; }

    public void SetData(params string[] input)
    {
        waveId = input[0];
        waveNumber = int.Parse(input[1]);
        enemyId = input[2];
        enemyCount = int.Parse(input[3]);
    }
}
