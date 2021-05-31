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

    float interactRange = 7.5f;
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
        DisableTaskPopUp();
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, taskLayerMask))
        {       
            hitTaskID = hit.transform.GetComponent<TaskID>().taskID;
            if ((database.tasks[hitTaskID].gameTask) && (database.tasks[hitTaskID].taskCompleted == false))
            {
                EnableTaskPopUp();
                taskText.text = database.tasks[hitTaskID].name;
                if (Input.GetKey(KeyCode.E))
                {
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
                TimerOff();
                DisableTaskPopUp();
                circleFill = 0;
                gControl.setTaskComplete(hitTaskID);
            }
        }

    }

    public void EnableTaskPopUp()
    {
        taskPopup.gameObject.SetActive(true);
        interactCircle.gameObject.SetActive(true);
    }

    public void DisableTaskPopUp()
    {
        taskPopup.gameObject.SetActive(false);
        interactCircle.gameObject.SetActive(false);
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
