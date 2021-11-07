using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    private int fps = 30;

    private Queue<float> fpsList;
    private int indexNum = 0;

    private void Start()
    {
        fpsList = new Queue<float>();
        for (int i = 0; i < 20; i++)
            fpsList.Enqueue(100);
    }


    void OnGUI()
    {
        float newFps = getCurrentFPS();
        fpsList.Enqueue(newFps);
        fpsList.Dequeue();
        fps = (int) fpsList.Average();
        GUI.Label(
            new Rect(Screen.width - 250 - 20, 20, 250, 50),
            "FPS: " + fps.ToString()
        );
    }


    float getCurrentFPS()
    {
        return 1.0f / Time.smoothDeltaTime;
    }
}