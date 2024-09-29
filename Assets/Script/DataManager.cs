using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class DataManager
{
    //read all csv in list
    public static void LoadCSVData(List<string> fileNameList)
    {
        foreach (string fileName in fileNameList)
        {
            switch (fileName)
            {
                case "Enemy":
                    {
                        //Game.SetAchievementList(SetData<Achievement>(fileName));
                        Game.SetEnemyList(SetData<Enemy>(fileName));
                    }
                    break;
                case "Wave":
                    {
                        //GameData.SetGameButtonList(SetData<GameButton>(fileName));
                        Game.SetWaveDataList(SetData<WaveData>(fileName));
                    }
                    break;
            }
        }
    }

    //read data from csv and set to objects of IDataClass type
    public static List<T> SetData<T>(string fileName) where T : IDataClass, new()
    {
        //read only files in given folder
        string filePath = Path.Combine(Application.streamingAssetsPath, "Data/" + fileName + ".csv");

        string[] dataArray = File.ReadAllLines(filePath);

        List<T> dataList = new List<T>();
        for (int i = 1; i < dataArray.Length; i++)
        {
            //read line by line and set to given type
            T t = new T();
            t.SetData(SplitCSVLine(dataArray[i]));
            dataList.Add(t);
        }

        return dataList;
    }

    //CSV parser: handles quotation marks and commas in strings
    private static string[] SplitCSVLine(string line)
    {
        //Debug.Log("SplitCSVLine " + line);
        List<string> splitLine = new List<string>();
        bool isString = false;
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '\"')
            {
                //is quote
                if (i > 0 && line[i - 1] == '\"')
                {
                    //is double quote
                    if (isString)
                    {
                        //start of line
                        if (splitLine.Count == 0) splitLine.Add("");

                        //continue string
                        splitLine[splitLine.Count - 1] += c;
                    }
                }
                isString = !isString;
            }
            else
            {
                //not quote
                if (c == ',')
                {
                    //is comma
                    if (!isString)
                    {
                        //new string
                        splitLine.Add("");
                    }
                    else
                    {
                        //start of line
                        if (splitLine.Count == 0) splitLine.Add("");

                        //continue string
                        splitLine[splitLine.Count - 1] += c;
                    }
                }
                else
                {
                    //start of line
                    if (splitLine.Count == 0) splitLine.Add("");

                    //continue string
                    splitLine[splitLine.Count - 1] += c;
                }
            }

        }

        return splitLine.ToArray();
    }
}

public interface IDataClass
{
    void SetData(params string[] input); //function to set data
}
