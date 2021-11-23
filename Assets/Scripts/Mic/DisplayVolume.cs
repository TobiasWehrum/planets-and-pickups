using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DisplayVolume : MonoBehaviour
{
    private TMP_Text t;


    
    private float v = 0;
    private float maxVol = 0;
    private float minVol = Mathf.Infinity;
    private float meadian;
    

    // Start is called before the first frame update
    void Start()
    {
        t = this.GetComponent<TMP_Text>();
        
        InvokeRepeating("UpdateMic", 3.0f, 0.2f);
    }

    

    void UpdateMic()
    { 
        v = MicInput.loudnessInDb;
        meadian = MicInput.medianLoudnessInDb;

        if (v > maxVol)
            maxVol = v;

        if (v < minVol)
            minVol = v;

        t.text = String.Format(
            "Volume: {0}\n" +
            "Max: {1}\n" +
            "Min: {2}", Mathf.RoundToInt(v), maxVol, minVol);
    }
}