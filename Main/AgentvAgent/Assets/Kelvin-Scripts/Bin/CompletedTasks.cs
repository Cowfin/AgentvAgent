using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int currentTaskNumber;
    GameController gameController;

    [SerializeField] Text taskNumber;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
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
