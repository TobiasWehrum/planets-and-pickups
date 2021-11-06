using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventExample : MonoBehaviour
{

    
    
    void Start ()
    {
        //Select the instance of AudioProcessor and pass a reference
        //to this object
        AudioProcessor processor = FindObjectOfType<AudioProcessor> ();
        processor.onBeat.AddListener (onOnbeatDetected);
        processor.onSpectrum.AddListener (onSpectrum);
    }

    

    //this event will be called every time a beat is detected.
    //Change the threshold parameter in the inspector
    //to adjust the sensitivity
    void onOnbeatDetected ()
    {
        Debug.Log ("Beat!!!");
    }

    //This event will be called every frame while music is playing
    void onSpectrum (float[] spectrum)
    {
        //The spectrum is logarithmically averaged
        //to 12 bands

        for (int i = 0; i < spectrum.Length; ++i) {
            Vector3 start = new Vector3 (i, 0, 0);
            Vector3 end = new Vector3 (i, spectrum [i], 0);
            Debug.DrawLine (start, end);
        }
    }
}
