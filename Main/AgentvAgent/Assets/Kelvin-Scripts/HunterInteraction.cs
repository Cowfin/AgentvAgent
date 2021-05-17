using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HunterInteraction : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Image tunnelVision;
    [SerializeField] Image tunnelVisionGrey;
    [SerializeField] AudioSource gunShot;

    float interactRange = 1000f;
    int taskLayerMask;

    bool obscureVision = true;
    bool fadeIn;
    bool fadeOut;

    float fadeInTime = 0.1f;
    float holdTime = 7f;
    float fadeOutTime = 3f;
    float fadeTimer = 0f;

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

        if (obscureVision == true)
        {
            fadeTimer += Time.deltaTime;
            if (fadeIn)
            {

                Color newTunnelVision = tunnelVision.color;
                newTunnelVision.a = fadeTimer / fadeInTime;
                tunnelVision.color = newTunnelVision;

                Debug.Log("Fading in, alpha : " + newTunnelVision.a);

                Color newTunnelVisionGrey = tunnelVisionGrey.color;
                newTunnelVisionGrey.a = 0.6f * (fadeTimer / fadeInTime);
                tunnelVisionGrey.color = newTunnelVisionGrey;

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
                    Color newTunnelVision = tunnelVision.color;
                    newTunnelVision.a -= fadeTimer / fadeOutTime;
                    tunnelVision.color = newTunnelVision;

                    Color newTunnelVisionGrey = tunnelVisionGrey.color;
                    newTunnelVisionGrey.a -= 0.6f * (fadeTimer / fadeOutTime);
                    tunnelVisionGrey.color = newTunnelVisionGrey;

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
                //if player hit then kill player

                resetTunnelVision();
                obscureVision = true;
                Debug.Log("Hunter Shot");
                // shoot but no one dies
            }

            if (Physics.Raycast(ray, out hit, interactRange, taskLayerMask))
            {
                
            }
        }
    }
    void resetTunnelVision()
    {
        Color newTunnelVision = tunnelVision.color;
        newTunnelVision.a = 0;
        tunnelVision.color = newTunnelVision;

        Color newTunnelVisionGrey = tunnelVisionGrey.color;
        newTunnelVisionGrey.a = 0;
        tunnelVisionGrey.color = newTunnelVisionGrey;

        fadeIn = true;
        fadeOut = false;
        fadeTimer = 0f;
    }
}