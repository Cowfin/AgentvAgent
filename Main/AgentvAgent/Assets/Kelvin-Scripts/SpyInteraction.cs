using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpyInteraction : MonoBehaviour
{
    [SerializeField] Image taskPopup;
    [SerializeField] Text taskText;
    [SerializeField] Image interactCircle;
    [SerializeField] Camera cam;

    float interactRange = 10f;
    int taskLayerMask;

    GameObject gameController;
    TaskDatabase database;
    GControl gControl;


    int hitTaskID;

    float totalTime, timerTime, circleFill;
    bool timerStatus;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GControl>();
        database = gameController.GetComponent<TaskDatabase>();

        taskLayerMask = LayerMask.GetMask("TaskLayer");
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, taskLayerMask))
        {
            EnableTaskPopUp();
            hitTaskID = hit.transform.GetComponent<TaskID>().taskID;
            taskText.text = database.tasks[hitTaskID].name;
            if (Input.GetKey(KeyCode.E))
            {
                if (hitTaskID == 0) {
                    Debug.Log("Pressed E on for blend in animation");
                }
                else {
                Debug.Log("Pressed E on task");
                Debug.Log("Time got: " + database.tasks[hitTaskID].completeTime);
                totalTime = database.tasks[hitTaskID].completeTime;
                TimerOn();
                }
            }
        }
        else
        {
            DisableTaskPopUp();
            TimerOff();
        }


        if (timerStatus)
        {
            timerTime += Time.deltaTime;
            circleFill = timerTime / totalTime;
            interactCircle.fillAmount = circleFill;
            if (circleFill >= 1)
            {
                DisableCircle();
                circleFill = 0;
                gControl.AddTaskNumber();
            }
        }

    }


    void EnableTaskPopUp()
    {
        taskPopup.gameObject.SetActive(true);
        interactCircle.gameObject.SetActive(true);
    }

    void DisableTaskPopUp()
    {
        taskPopup.gameObject.SetActive(false);
        interactCircle.gameObject.SetActive(false);
    }

    void DisableCircle()
    {
        interactCircle.gameObject.SetActive(false);
        TimerOff();
    }

    public void TimerOn()
    {
        timerStatus = true;
    }

    public void TimerOff()
    {
        timerStatus = false;
        timerTime = 0;
        interactCircle.fillAmount = 0;
    }

    public void SetTime(float t)
    {
        totalTime = t;
    }
}
