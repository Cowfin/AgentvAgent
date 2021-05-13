using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleTasks : MonoBehaviour
{
    GameObject gameController;
    TaskDatabase database;

    public void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        database = gameController.GetComponent<TaskDatabase>();
    }

    public List<Task> randomiseTask(int max)
    {
        List<Task> randomTasks = new List<Task>();
        List<Task> taskList = database.getTaskList();
        int index;

        for (int i = 0; i < max; i++)
        {
            index = UnityEngine.Random.Range(1, taskList.Count);
            randomTasks.Add(taskList[index]);
            taskList.RemoveAt(index);
        }

        return randomTasks;
    }

}