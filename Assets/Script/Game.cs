using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a class to store all the static object that can be access anywhere in the scene
public static class Game
{
    #region HUD
    private static HUDController hudController;
    public static HUDController GetHUDController() => hudController;
    public static void SetHUDController(HUDController hc) => hudController = hc;
    #endregion

    #region gamecontroller
    private static GameController gameController;
    public static GameController GetGameController() => gameController;
    public static void SetGameController(GameController gc) => gameController = gc;
    #endregion

    #region player
    private static PlayerController mainPlayer;
    public static PlayerController GetPlayer() => mainPlayer;
    public static void SetPlayer(PlayerController Player) => mainPlayer = Player;
    #endregion

    #region soundManager
    private static SoundManager soundManager;
    public static SoundManager GetSoundManager() => soundManager;
    public static void SetSoundManager(SoundManager sm) => soundManager = sm;
    #endregion

    #region enemy
    public static List<Enemy> enemyList;
    // Enemy Set and Get
    public static Enemy GetEnemyByRefID(string id)
    {
        return enemyList.Find(x => x.enemyId == id);
    }
    public static List<Enemy> GetEnemyList()
    {
        return enemyList;
    }
    public static void SetEnemyList(List<Enemy> eList)
    {
        enemyList = eList;
        Debug.Log("Setting enemy list");
    }
    #endregion

    #region enemyWave
    public static List<WaveData> waveDataList;
    public static WaveData GetWaveByRefID(string waveID)
    {
        return waveDataList.Find(x => x.waveId == waveID);
    }
    public static List<WaveData> GetWaveDataList()
    {
        return waveDataList;
    }
    public static void SetWaveDataList(List<WaveData> wDList)
    {
        waveDataList = wDList;
    }
    #endregion

    #region wave
    private static WaveManager waveManager;
    public static WaveManager GetWaveManager() => waveManager;
    public static void SetWaveManager(WaveManager wave) => waveManager = wave;
    #endregion

    #region enemySpawner
    private static EnemySpawner enemySpawner;
    public static EnemySpawner GetEnemySpawner() => enemySpawner;
    public static void SetEnemySpawner(EnemySpawner es) => enemySpawner = es;
    #endregion

    
    //public static void SetDialogueList(List <Dialogue> dList)
    //{
    //    dialogueList = dList;
    //}
    //#endregion
    //public static string GetSystemTime()
    //{
    //    return System.DateTime.Now.ToString();
    //}
}
