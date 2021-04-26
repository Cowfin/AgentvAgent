using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCricle : MonoBehaviour
{
    Image img;

    float totalTime = 2;
    bool timerStatus;
    float timerTime;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (timerStatus)
        {
            timerTime += Time.deltaTime;
            img.fillAmount = timerTime / totalTime;
        }
    }

    public void timerOn()
    {
        timerStatus = true;
    }

    public void timerOff()
    {
        timerStatus = false;
        timerTime = 0;
        img.fillAmount = 0;
    }

    public void setTime(float t)
    {
        totalTime = t;
    }

    public void setImg(Image i)
    {
        img = i;
    }

}
