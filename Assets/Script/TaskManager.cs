using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TaskManager : MonoBehaviour
{
    private Queue<Func<bool>> taskQueue;
    private Queue<string> instructionQueue;
    private Dictionary<TaskType, bool> tasksList = new Dictionary<TaskType, bool>();
    private void Awake()
    {
        Game.SetTaskManager(this);

        //initialise the tasklist
        tasksList.Add(TaskType.PLAYER_WARMED, false);
        tasksList.Add(TaskType.FIRE_REFUELLED_ONCE, false);
        tasksList.Add(TaskType.FIRE_REFUELLED_AGAIN, false);
    }

    public bool IsTaskCompleted(TaskType taskType)
    {
        if (tasksList.ContainsKey(taskType))
        {
            return tasksList[taskType];
        }
        else
            return false;
    }

    public void SetTaskCompleted(TaskType taskType)
    {
        if (tasksList.ContainsKey(taskType) && tasksList[taskType] != true)
        {
            tasksList[taskType] = true;
            Game.GetHUDController().HideInstructions();
        }
    }

    public bool AreAllTasksCompleted()
    {
        bool completedAll = false;

        foreach (KeyValuePair<TaskType, bool> kvp in tasksList)
        {
            Console.WriteLine("Key: {0}, Value: {1}", kvp.Key, kvp.Value);
            if (kvp.Value == false)
            {
                completedAll = false;
            }
            else
            {
                completedAll = true;
            }
        }

        return completedAll;
    }
}

public enum TaskType
{
    PLAYER_WARMED,
    FIRE_REFUELLED_ONCE,
    FIRE_REFUELLED_AGAIN
}
