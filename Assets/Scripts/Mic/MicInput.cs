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
        UpdateCache();
        loudness = GetLoudness();
        loudnessInDb = GetLoudnessInDb();
        medianLoudnessInDb = getMedianLoudness();
        Debug.Log(
            "Mic loudness: median " + 
            medianLoudnessInDb.ToString("####") + 
            " dB, curr " + loudnessInDb.ToString("####")
            );
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
    }

    //stop mic when loading a new level or quit application
    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
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