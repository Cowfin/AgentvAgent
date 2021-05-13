using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskWin
{
    public bool checkSpyTaskWin(int taskToCompleteNumber, int taskCompletedNumber)
    {
        return taskToCompleteNumber <= taskCompletedNumber;
    }
}
