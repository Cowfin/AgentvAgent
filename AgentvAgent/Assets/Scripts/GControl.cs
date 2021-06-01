/*
 * GControl Class is the main controller of the game.
 * It handles the game timer, tasks randomisation, task completion, and winning conditions.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private int[] taskIDList = new int[TOTAL_TASK_NUMBER];
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

    private bool transitionTimer;
    private float transitionTime = 5;

    void Start()
    {
        database = gameObject.GetComponent<TaskDatabase>();
        spyTaskCompleted = 0;
        endGamePopupHide();
        createTaskList();
        randomiseTask(TOTAL_TASK_NUMBER);
        assignTaskList();
        updateDatabase();
        setTimeRemaining(240);
        startTime();
        updateTime();
        transitionTimer = false;
        transitionTime = 5;
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

        if (transitionTimer)
        {
            if(transitionTime > 0)
            {
                timeRemaining -= Time.deltaTime;
            } else
            {
                SceneManager.LoadScene(0);
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
        int seconds = (int)timeRemaining % 60;
        if (seconds < 10)
        {
            timerText.text = ((int)timeRemaining / 60).ToString() + ":0" + ((int)timeRemaining % 60).ToString();
        } else
        {
            timerText.text = ((int)timeRemaining / 60).ToString() + ":" + ((int)timeRemaining % 60).ToString();
        }
    }

    public void endGamePopupShow()
    {
        endGamePopup.gameObject.SetActive(true);
        transitionTimer = true;
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
        for (int i = 0; i < taskIDList.Length; i++)
        {
            if (taskID == (taskIDList[i]))
            {
                index = i;
                break;
            }
        }
        database.tasks[taskIDList[index]].taskCompleted = true;
        gameTaskListText[index].color = Color.red;
        addTaskComplete();
        if (checkSpyTaskWin())
        {
            endGameSpyTaskWin();
        }
    }

    public void addTaskComplete()
    {
        spyTaskCompleted++;
    }

    public void randomiseTask(int max)
    {
        int taskCount = database.getTaskList().Count;
        int rand;
        int counter = 0;
        while(counter < max)
        {
            rand = UnityEngine.Random.Range(1, taskCount);
            if (arrayContains(taskIDList, rand)){
                taskIDList[counter] = rand;
                counter++;
            }
        }
        
    }

    public bool arrayContains(int[] arr, int check)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == check)
            {
                return false;
            }
        }
        return true;
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
        for (int i = 0; i < taskIDList.Length; i++)
        {
            gameTaskList.Add(database.tasks[taskIDList[i]]);
            gameTaskListText[i].text = gameTaskList[i].location.ToString() + ":" + gameTaskList[i].name.ToString();
        }
    }
    
    public List<Task> getTaskList()
    {
        return gameTaskList;
    }

    public void updateDatabase()
    {
        for (int i = 0; i < taskIDList.Length; i++)
        {
            for (int j = 0; j < database.tasks.Count; j++)
            {
                if (taskIDList[i] == database.tasks[j].taskID)
                {
                    database.tasks[j].gameTask = true;
                }
            }
        }
    }

    public bool checkSpyTaskWin()
    {
        return (spyTaskCompleted >= NUMBER_TASKS_TO_COMPLETE);
    }

    public bool checkTimeWin()
    {
        return (timeRemaining <= 0);
    }
}
