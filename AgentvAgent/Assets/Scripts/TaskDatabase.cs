/*
 * The class stores a list of tasks that can be passed around and updated.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDatabase : MonoBehaviour
{
    public List<Task> tasks = new List<Task>();

    public List<Task> getTaskList()
    {
        return this.tasks;
    }
}
