using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskInteraction : MonoBehaviour 
{
    [SerializeField] Image taskPopup;

    float interactRange = 10f;

    int taskLayerMask;

    Camera cam;

    GameObject gameController;
    TaskDatabase database;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        database = gameController.GetComponent<TaskDatabase>();
        cam = gameController.GetComponent<Camera>();

        taskLayerMask = LayerMask.GetMask("TaskLayer");
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, taskLayerMask))
        {
            EnableTaskPopUp();
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Pressed E on task");
                int id = hit.transform.GetComponent<TaskID>().taskID;

            }

        }
        else
        {
            DisableTaskPopUp();
        }

    }

    void EnableTaskPopUp()
    {
        taskPopup.gameObject.SetActive(true);
    }

    void DisableTaskPopUp()
    {
        taskPopup.gameObject.SetActive(false);
    }
}
