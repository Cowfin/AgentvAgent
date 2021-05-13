using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GControl : MonoBehaviour
{
    int currentTaskNumber;
    GameController gameController;
    [SerializeField] Text taskNumber;

    private float timeRemaining;
    private bool timerRunning;
    [SerializeField] Text timerText;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        currentTaskNumber = 0;
        UpdateTaskNumber();
        setTimeRemaining(360);
        startTime();
        updateTime();
    }

    void Update()
    {
        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                updateTime();
            }
            else
            {
                stopTime();
                timeRemaining = -1;
            }
        }
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
    public void setTimeRemaining(float time)
    {
        this.timeRemaining = time;
    }

    public void startTime()
    {
        this.timerRunning = true;
    }

    public void stopTime()
    {
        this.timerRunning = false;
    }

    public void updateTime()
    {
        timerText.text = ((int)timeRemaining / 60).ToString() + ":" + ((int)timeRemaining % 60).ToString();
    }

}
