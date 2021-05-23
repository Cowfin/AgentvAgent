using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDatabase : MonoBehaviour
{
    public List<Task> tasks = new List<Task>();

    public List<Task> getTaskList()
    {
        List<Task> tl = tasks;
        return tl;
    }
}
