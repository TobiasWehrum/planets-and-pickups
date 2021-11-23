using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicTrigger : MonoBehaviour
{
    public Transform MaxAmpSprite;
    public Transform CurrAmpSprite;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Update()
    {

        var lenMaxBar = MaxAmpSprite.localScale.magnitude;
        var loudness = MicInput.loudnessInDb/ MicInput.medianLoudnessInDb;

        if (loudness < 500)
        {
            var normedLoudness = loudness * lenMaxBar;
            CurrAmpSprite.localScale = new Vector3(
                normedLoudness,
                1.0f,
                1.0f
            );

        }



    }
}