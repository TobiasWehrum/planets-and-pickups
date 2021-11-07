/**
 * Check your Microphone Permissions, especially on Mac. 
 * I had to delete Unity Hub, run the unity project, allow mic access when the warning pops up,
 * and then reinstall Unity Hub or restore it from the trash after mic access is granted. 
 */

using UnityEngine;



public class TestMicInput : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f + (MicInput.loudness * 10.0f), 1.0f);
    }

}