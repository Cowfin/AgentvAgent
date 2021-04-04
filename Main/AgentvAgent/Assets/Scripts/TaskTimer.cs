using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTimer : MonoBehaviour {

    public float timeRemaining;
    public bool timerRunning = false;

    void Start() {
        timerRunning = true;
    }

    void Update() {
        if (timerRunning) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            } else {
                timerRunning = false;
                timeRemaining = 0;
            }
        }
    }
}
