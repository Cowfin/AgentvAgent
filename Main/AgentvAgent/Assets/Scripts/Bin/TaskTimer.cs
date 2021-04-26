using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTimer : MonoBehaviour {

    float timeRemaining;
    private bool timerRunning = false;
    [SerializeField] Image timerPopup;

    void Start() {
        timerRunning = true;
    }

    void Update() {
        if (timerRunning) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            } else {
                stopTimer();
                timeRemaining = -1;
            }
        }
    }
    public void startTimer()
    {
        timerRunning = true;
        timeRemaining = 5;
    }

    public void stopTimer()
    {
        timerRunning = false;
    }

    public void setTime(float newTime)
    {
        timeRemaining = newTime;
    }

}
