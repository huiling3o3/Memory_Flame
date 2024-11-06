using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsTrigger : MonoBehaviour
{
    [SerializeField] private TaskManager tm;
    [SerializeField] TaskType taskType;
    [SerializeField] string Instructions;
    // Start is called before the first frame update
    void Start()
    {
        tm =  Game.GetTaskManager();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Check if the task have been completed, if not prompt the instructions
            if (!tm.IsTaskCompleted(taskType))
            {
                Game.GetHUDController().ShowInstructions(Instructions);
            }                       
        }
    }
}
