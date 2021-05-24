using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterInteraction : MonoBehaviour
{
    [SerializeField] Camera cam;

    float interactRange = 1000f;
    int taskLayerMask;

    GameObject gameController;
    TaskDatabase database;
    GControl gControl;

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

        if (Input.GetMouseButtonDown(1)) // On right click
        {
            Debug.Log("Hunter Aiming");
            //Aiming

            if (Physics.Raycast(ray, out hit, interactRange, taskLayerMask))
            {
                if (Input.GetMouseButton(0))
                {
                    //if player hit then kill player
                } else
                {
                    // shoot but no one dies
                }
                
            }
            
        }

       



    }

}
