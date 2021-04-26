﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Image taskPopup;
    [SerializeField] Image interactCircle;

    float interactRange = 10f;
    int taskLayerMask;
    [SerializeField] Camera cam;

    GameObject gameController;
    TaskDatabase database;

    int hitTaskID;
    //TaskTimer taskTimer;
    //TestCricle testCricle;


    //Image img;

    float totalTime = 2;
    bool timerStatus;
    float timerTime;


    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
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
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("Pressed E on task");
                hitTaskID = hit.transform.GetComponent<TaskID>().taskID;
                Debug.Log("Time got: " + database.tasks[hitTaskID].completeTime);
                timerOn();
                /*testCricle.setImg(interactCircle);
                testCricle.setTime(database.tasks[hitTaskID].completeTime);
                testCricle.timerOn();*/

               /* if (database.tasks[hitTaskID].taskID == 1)
                {
                    taskTimer.startTimer();
                }
                //taskTimer.setTime(database.tasks[hitTaskID].completeTime);
                //taskTimer.startTimer();*/
                
            }

        } else
        {
            DisableTaskPopUp();
            timerOff();
        }


        if (timerStatus)
        {
            timerTime += Time.deltaTime;
            interactCircle.fillAmount = timerTime / totalTime;
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



    //testing circle

    public void timerOn()
    {
        timerStatus = true;
    }

    public void timerOff()
    {
        timerStatus = false;
        timerTime = 0;
        interactCircle.fillAmount = 0;
    }

    public void setTime(float t)
    {
        totalTime = t;
    }
}
