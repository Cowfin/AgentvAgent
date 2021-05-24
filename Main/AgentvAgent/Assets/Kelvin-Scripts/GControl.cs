using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GControl : MonoBehaviour
{
    int currentTaskNumber;

    [SerializeField] Text taskNumber;

    void Start()
    {
        currentTaskNumber = 0;
        UpdateTaskNumber();
    }

    void Update()
    {

    }

    public void AddTaskNumber()
    {
        currentTaskNumber++;
        UpdateTaskNumber();
    }

    public void UpdateTaskNumber()
    {
        taskNumber.text = currentTaskNumber.ToString("0");
    }
}
