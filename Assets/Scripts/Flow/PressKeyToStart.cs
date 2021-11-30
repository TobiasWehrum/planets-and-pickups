using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A script that when enabled stops time, waits for a key to be pressed and then hides itself and restores the flow of time.
    /// </summary>
    public class PressKeyToStart : MonoBehaviour
    {
        [SerializeField] KeyCode key = KeyCode.Space;

        void OnEnable()
        {
            Time.timeScale = 0f;
        }
        
        void OnDisable()
        {
            Time.timeScale = 1f;
        }
        
        void Update()
        {
            if (Input.GetKeyDown(key)|| MicInput.isTriggered())
            {
                gameObject.SetActive(false);
            }
        }
    }
}