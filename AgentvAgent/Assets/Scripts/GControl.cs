using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GControl : MonoBehaviour
{
    TaskDatabase database;

    private float timeRemaining;
    private bool timerRunning;
    [SerializeField] Text timerText;

    [SerializeField] Image endGamePopup;
    [SerializeField] Text endGamePopupText;

    private static int TOTAL_TASK_NUMBER = 10;
    private static int NUMBER_TASKS_TO_COMPLETE = 6;
    private int[] taskIDList = new int[TOTAL_TASK_NUMBER]; //use this for comparing task ids
    private List<Task> gameTaskList = new List<Task>();
    private List<Text> gameTaskListText = new List<Text>();

    [SerializeField] Text textTask1;
    [SerializeField] Text textTask2;
    [SerializeField] Text textTask3;
    [SerializeField] Text textTask4;
    [SerializeField] Text textTask5;
    [SerializeField] Text textTask6;
    [SerializeField] Text textTask7;
    [SerializeField] Text textTask8;
    [SerializeField] Text textTask9;
    [SerializeField] Text textTask10;

    private int spyTaskCompleted;

    void Start()
    {
        database = gameObject.GetComponent<TaskDatabase>();
        spyTaskCompleted = 0;
        endGamePopupHide();
        /*createTaskList();
        randomiseTask(TOTAL_TASK_NUMBER);
        assignTaskList();
        updateDatabase();*/
        setTimeRemaining(100);
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
                endGameHunterTimerWin();
                timeRemaining = -1;
            }
        }
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

    public void setTaskComplete(int taskID)
    {
        int index = 0;
        for (int i = 0; i < gameTaskList.Count; i++)
        {
            if (taskID == (gameTaskList[i].taskID))
            {
                index = i;
                break;
            }
        }
        gameTaskList[index].taskCompleted = true;
        gameTaskListText[index].color = Color.red;
        spyTaskCompleted++;
        checkSpyTaskWin();
    }

    public void randomiseTask(int max)
    {
        gameTaskList = database.getTaskList();
        gameTaskList.RemoveAt(0);
        while (gameTaskList.Count > max)
        {
            gameTaskList.RemoveAt(UnityEngine.Random.Range(0, gameTaskList.Count));
        }

    }

    public void createTaskList()
    {
        gameTaskListText.Add(textTask1);
        gameTaskListText.Add(textTask2);
        gameTaskListText.Add(textTask3);
        gameTaskListText.Add(textTask4);
        gameTaskListText.Add(textTask5);
        gameTaskListText.Add(textTask6);
        gameTaskListText.Add(textTask7);
        gameTaskListText.Add(textTask8);
        gameTaskListText.Add(textTask9);
        gameTaskListText.Add(textTask10);
    }

    public void assignTaskList()
    {
        for (int i = 0; i < gameTaskList.Count; i++)
        {
            gameTaskListText[i].text = gameTaskList[i].location.ToString() + ":" + gameTaskList[i].name.ToString();
        }
    }

    public void updateDatabase()
    {
        for (int i = 0; i < gameTaskList.Count; i++)
        {
            for (int j = 0; j < database.tasks.Count; j++)
            {
                if (gameTaskList[i].taskID == database.tasks[j].taskID)
                {
                    database.tasks[j].gameTask = true;
                }
            }
        }
    }

    public bool checkSpyTaskWin()
    {
        return (spyTaskCompleted == NUMBER_TASKS_TO_COMPLETE);
    }
}
