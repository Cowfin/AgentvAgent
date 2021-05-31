using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HunterInteraction : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Image tunnelVision;
    [SerializeField] Image tunnelVisionGrey;
    [SerializeField] Image exclamationMark;
    [SerializeField] Image crossIcon;
    [SerializeField] AudioSource gunShot;
    [SerializeField] AudioSource crowdScream;
    [SerializeField] GameObject abilityUITarget;

    float interactRange = 1000f;
    int taskLayerMask;

    bool obscureVision = true;
    bool fadeIn;
    bool fadeOut;

    float fadeInTime = 0.1f;
    float holdTime = 10f;
    float fadeOutTime = 3f;
    float fadeTimer = 0f;

    float abilityIconDisplayTime = 3f;
    float abilityIconTimer = 0f;

    Color newTunnelVision;
    Color newTunnelVisionGrey;

    GameObject gameController;
    TaskDatabase database;
    GControl gControl;
    GameObject spy;


    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GControl>();
        database = gameController.GetComponent<TaskDatabase>();
        spy = GameObject.FindGameObjectWithTag("Spy");
        taskLayerMask = LayerMask.GetMask("TaskLayer");
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        if (abilityIconTimer >= 0)
        {
            Vector3 iconPos = Camera.current.WorldToScreenPoint(abilityUITarget.transform.position);
            exclamationMark.transform.position = iconPos;
            crossIcon.transform.position = iconPos;
            abilityIconTimer -= Time.deltaTime;
        }
        else 
        {
            exclamationMark.enabled = false;
            crossIcon.enabled = false;
        }

        if (obscureVision == true)
        {
            fadeTimer += Time.deltaTime;
            if (fadeIn)
            {
                tunnelVision.color = new Color(tunnelVision.color.r, tunnelVision.color.g, tunnelVision.color.b, fadeTimer / fadeInTime);
                tunnelVisionGrey.color = new Color(tunnelVisionGrey.color.r, tunnelVisionGrey.color.g, tunnelVisionGrey.color.b, 0.6f * (fadeTimer / fadeInTime));

                if (fadeTimer >= fadeInTime)
                {
                    fadeIn = false;
                    fadeTimer = 0;
                }
            }
            else
            {
                if (fadeOut == false)
                {
                    if (fadeTimer >= holdTime)
                    {
                        fadeOut = true;
                        fadeTimer = 0;
                    }
                }
                else
                {
                    tunnelVision.color = new Color(tunnelVision.color.r, tunnelVision.color.g, tunnelVision.color.b, tunnelVision.color.a - (fadeTimer / fadeInTime));
                    tunnelVisionGrey.color = new Color(tunnelVisionGrey.color.r, tunnelVisionGrey.color.g, tunnelVisionGrey.color.b, tunnelVisionGrey.color.a - (0.6f * (fadeTimer / fadeInTime)));

                    if (fadeTimer >= fadeOutTime)
                    {
                        fadeOut = false;
                        fadeTimer = 0;
                        obscureVision = false;
                    }
                }
            }
        }

        if (Input.GetMouseButton(1)) // On right click
        {
            Debug.Log("Hunter Aiming");
            //Aiming
            if (Input.GetMouseButtonDown(0))
            {
                gunShot.Play();
                crowdScream.Play();
                //if player hit then kill player

                resetTunnelVision();
                obscureVision = true;
                Debug.Log("Hunter Shot");
                // shoot but no one dies
            }

            /*if (Physics.Raycast(ray, out hit, interactRange, taskLayerMask))
            {

            }*/
        }

        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            bool spyClose = HunterAbility.CheckSpyInRange(7.5f, this.transform.position, spy.transform.position);
            if (spyClose)
            {
                exclamationMark.enabled = true;
            }
            else 
            {
                crossIcon.enabled = true;
            }
            abilityIconTimer = abilityIconDisplayTime;
        }
    }
    void resetTunnelVision()
    {
        tunnelVision.color = new Color(tunnelVision.color.r, tunnelVision.color.g, tunnelVision.color.b, 0);
        tunnelVisionGrey.color = new Color(tunnelVisionGrey.color.r, tunnelVisionGrey.color.g, tunnelVisionGrey.color.b, 0);

        fadeIn = true;
        fadeOut = false;
        fadeTimer = 0f;
    }
}