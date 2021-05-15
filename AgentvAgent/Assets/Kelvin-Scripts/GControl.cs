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

    [SerializeField] Image endGamePopup;
    [SerializeField] Text endGamePopupText;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        currentTaskNumber = 0;
        UpdateTaskNumber();
        setTimeRemaining(10);
        startTime();
        updateTime();
        endGamePopupHide();
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
                endGameHunterTimerWin();
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

    public void endGamePopupShow()
    {
        endGamePopup.gameObject.SetActive(true);
    }

    public void endGamePopupHide()
    {
        endGamePopup.gameObject.SetActive(false);       
    }

    public void endGameHunterTimerWin()
    {
        endGamePopupText.text = "Hunter Wins \nSpy Ran Out Of Time";
        endGamePopupShow();
    }

    public void endGameHunterKillWin()
    {
        endGamePopupText.text = "Hunter Wins \nHunter caught the spy";
        endGamePopupShow();
    }

    public void endGameSpyTaskWin()
    {
        endGamePopupText.text = "Spy Wins \nSpy completed their tasks";
        endGamePopupShow();
    }

}
