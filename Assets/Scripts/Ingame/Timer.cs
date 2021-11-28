using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeRemaining;

    public bool timerIsRunning = false;


    public void Reset()
    {
        timeRemaining = 0;
        timerIsRunning = false;
    }

    public void StartTimer(int seconds)
    {
        timeRemaining = seconds;
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                Reset();
            }
        }
    }


    public int timeInSeconds()
    {
        return Mathf.FloorToInt(timeRemaining % 60);
    }
}