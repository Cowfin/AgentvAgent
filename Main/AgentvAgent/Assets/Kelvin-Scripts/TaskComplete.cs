using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskComplete : MonoBehaviour {
    public int[] tasksCompleted;
    public GameObject[] totalTasks;

    private void Start()
    {
        tasksCompleted = new int[2];
        totalTasks = new GameObject[2];
    }
}
