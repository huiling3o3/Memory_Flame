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

    //#region character
    ////Get a single character
    //public static Character GetCharacterByRefID(string id)
    //{
    //    return characterList.Find(x => x.id == id);
    //}

    //public static List<Character> GetCharacterList() => characterList;

    //public static void SetCharacterList(List<Character> cList) => characterList = cList;

    //#endregion

    //#region buff
    ///// <summary>
    ///// Buff Set and Get
    ///// </summary>
    ///// <param name="bList"></param>
    //public static void SetBuffList(List<Buff> bList) 
    //{
    //    buffList = bList;
    //}
    //public static List<Buff> GetBuffList() => buffList;
    //public static Buff GetBuffByRefID(string id)
    //{
    //    return buffList.Find(x => x.id == id);
    //}
    //#endregion

    //#region enemy
    ///// Enemy Set and Get
    //public static Enemy GetEnemyByRefID(string id)
    //{
    //    return enemyList.Find(x => x.id == id);
    //}
    //public static List<Enemy> GetEnemyList()
    //{
    //    return enemyList;
    //}
    //public static void SetEnemyList(List<Enemy> eList)
    //{
    //    enemyList = eList;
    //    Debug.Log("Setting enemy list");
    //}
    //#endregion

    //#region weapon
    //public static Weapon GetWeaponByRefID(string id)
    //{
    //    return weaponList.Find(x => x.id == id);
    //}
    //public static List<Weapon> GetWeaponList() => weaponList;
    //public static void SetWeaponList(List<Weapon> wList) => weaponList = wList;

    //#endregion

    //#region enemyWave
    //public static WaveData GetWaveByRefID(string waveID)
    //{
    //    return waveDataList.Find(x => x.waveID == waveID);
    //}
    //public static List<WaveData> GetWaveDataList()
    //{
    //    return waveDataList;
    //}
    //public static void SetWaveDataList(List<WaveData> wDList)
    //{
    //    waveDataList = wDList;
    //}
    //#endregion

    //#region barrel
    //public static Barrel GetBarrelByRefID(string id)
    //{
    //    return barrelList.Find(x => x.id == id);
    //}
    //public static List<Barrel> GetBarrelList() => barrelList;
    //public static void SetBarrelList(List<Barrel> bList) => barrelList = bList;

    //#endregion

    //#region dialogue
    //public static Dialogue GetDialogueByRefID(string cutsceneID)
    //{
    //    return dialogueList.Find(x => x.cutsceneID == cutsceneID);
    //}

    //public static List <Dialogue> GetDialogueList() 
    //{  
    //    return dialogueList; 
    //}

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
