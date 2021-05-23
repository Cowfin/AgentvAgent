using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GControl : MonoBehaviour
{
    GameController gameController;
    TaskDatabase database;
    ShuffleTasks shuffleTasks;

    private float timeRemaining;
    private bool timerRunning;
    [SerializeField] Text timerText;

    [SerializeField] Image endGamePopup;
    [SerializeField] Text endGamePopupText;

    private static int TOTAL_TASK_NUMBER = 10;
    private int[] taskIDList;
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
    

    void Start()
    {
        database = gameObject.GetComponent<TaskDatabase>();
        createTaskList();
        randomiseTask(TOTAL_TASK_NUMBER);
        assignTaskList();
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
        for (int i = 0; i <= gameTaskList.Count; i++)
        {
            if (taskID == taskIDList[i])
            {
                index = i;
                break;
            }
        }
        gameTaskList[index].taskCompleted = true;
        updateGameTaskList(index);
    }

    public void randomiseTask(int max)
    {
        /*List<Task> taskListTemp = database.getTaskList();
        //var tl = database.getTaskList();
        int index;
        int counter = taskListTemp.Count;
        for (int i = 0; i < max; i++)
        {
            index = UnityEngine.Random.Range(1, counter);
            Debug.Log("Random: " + index);
            //gameTaskList.Add(taskListTemp[index]);
            //gameTaskList.Add(tl[index]);
            //taskListTemp.RemoveAt(index);
            //tl.RemoveAt(index);
            counter--;
        }*/

        //why not remove instead of add? O_O
        gameTaskList = database.getTaskList();
        int index;
        Debug.Log("gtl: " + gameTaskList.Count);
        gameTaskList.RemoveAt(0);
        while(gameTaskList.Count > max)
        {
            index = UnityEngine.Random.Range(0, gameTaskList.Count - 1);
            Debug.Log("Random: " + index);
            //taskList.RemoveAt(UnityEngine.Random.Range(0, taskList.Count));
            gameTaskList.RemoveAt(index);
        }

        Debug.Log("gtl: " + gameTaskList.Count);

        /*for (int i = 0; i < gameTaskList.Count; i++)
        {
            //taskIDList[i] = gameTaskList[i].taskID;
            //Debug.Log("TaskID List: " + gameTaskList[i].taskID);
        }*/
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
        for (int i = 0; i <= gameTaskList.Count; i++)
        {
            gameTaskListText[i].text = gameTaskList[i].location.ToString() + ":" + gameTaskList[i].name.ToString();
        }
    }

    public void updateGameTaskList(int index)
    {
        gameTaskListText[index].color = Color.red;
    }

}
