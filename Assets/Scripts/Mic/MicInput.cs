using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




[RequireComponent(typeof(AudioSource))]
public class MicInput : MonoBehaviour
{
    #region SingleTon

    public static MicInput Instance { set; get; }

    #endregion

    public static float loudness;
    
    public static float loudnessInDb;
    public static float medianLoudnessInDb;
    public float maxLoudnessInDb=100;
    public float minLoudnessInDb=0;
    public bool trigger;
    
    

    AudioClip _clipRecord;
    AudioClip _recordedClip;
    int _sampleWindow = 128;
    bool _isInitialized;
    private string _device;

    public Queue<float> loudnessDBCache;
    

#if UNITY_WEBGL && !UNITY_EDITOR

    void InitMic()
    {
        Microphone.Init();
        Microphone.QueryAudioInput();
    }
    
    public float GetLoudness()
    {
        Microphone.Update();
        float max_vol = -10000;
        
         for(int i=0; i<Microphone.volumes.Length; i++)
         {
          if(max_vol < Microphone.volumes[i])
           max_vol = Microphone.volumes[i];
         }
        return max_vol;

    }

#else

    void InitMic()
    {
        
        if (_device == null)
        {
            _device = Microphone.devices[0];
        }
        _clipRecord = Microphone.Start(_device, true, 999, 44100);
    }

    //get data from microphone into audioclip
    public float GetLoudness()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition =
            Microphone.GetPosition(null) -
            (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }

        return levelMax;
    }


#endif


    void UpdateCache()
    {
        if (loudnessDBCache.Count < 20)
        {
            loudnessDBCache.Enqueue(loudnessInDb);
        }
        else
        {
            loudnessDBCache.Enqueue(loudnessInDb);
            loudnessDBCache.Dequeue();
        }
    }

    void Update()
    {
        
        loudness = GetLoudness();
        loudnessInDb = GetLoudnessInDb();

        if (loudnessInDb < 0.5 * medianLoudnessInDb)
        {
            trigger = true;
        }
        else
            trigger = false;
        
        if (!trigger)
            UpdateCache();
        
        medianLoudnessInDb = getMedianLoudness();
        UpdateGraphs();
        DebugGUI.LogPersistent("trigger", String.Format("Triggered: {0}", trigger));

    }


    public void StopMicrophone()
    {
        Microphone.End(_device);
        _isInitialized = false;
    }


    //get data from microphone into audioclip
    public float GetLoudnessInDb()
    {
        return Mathf.Abs( 20 * Mathf.Log10(Mathf.Abs(loudness)));
    }


    // start mic when scene starts
    void OnEnable()
    {
        loudnessDBCache = new Queue<float>();
        InitMic();
        _isInitialized = true;
        Instance = this;
        
        SetupDebuggers();
    }

    void SetupDebuggers()
    {
        // Set up graph properties using our graph keys
        DebugGUI.SetGraphProperties("MicInput", "Input", 0, 100, 0, new Color(0, 1, 1),  
        true);
        DebugGUI.SetGraphProperties("Median", "Median", 0, 100, 0, new Color(1, 0.5f, 1), true);
        DebugGUI.SetGraphProperties("Max", "Max", 0, 100, 0, new Color(1, 1, 0), true);
        DebugGUI.SetGraphProperties("Min", "Min", 0, 100, 0, new Color(1, 1, 0), true);
    }


    void UpdateGraphs()
    {
        DebugGUI.Graph(key:"MicInput", val:loudnessInDb);
        DebugGUI.Graph(key:"Median", val:medianLoudnessInDb);
        DebugGUI.Graph(key:"Max", val:maxLoudnessInDb);
        DebugGUI.Graph(key:"Min", val:minLoudnessInDb);
    }

    //stop mic when loading a new level or quit application
    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
        
        // Clean up our logs and graphs when this object is destroyed
        DebugGUI.RemoveGraph("MicInput");
        DebugGUI.RemoveGraph("Median");
        DebugGUI.RemoveGraph("Max");
        DebugGUI.RemoveGraph("Min");
        DebugGUI.RemovePersistent("trigger");
    }


    float getMedianLoudness()
    {
        
        int numberCount = loudnessDBCache.Count;
        int halfIndex = loudnessDBCache.Count/2;
        var sortedNumbers = loudnessDBCache.OrderBy(n=>n);
        float median;
        if (numberCount > 0)
        {
            if ((numberCount % 2) == 0)
            {
                float mid = sortedNumbers.ElementAt(halfIndex);
                float mid_1 =  sortedNumbers.ElementAt(halfIndex-1);
            
                median = (mid + mid_1)/ 2;
            } else {
                median = sortedNumbers.ElementAt(halfIndex);
            }
        }
        else
        {
            median = 1000;
        }
        

        return median;
    }
}