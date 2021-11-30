using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A script that waits for a key to be pressed and then reloads the current scene.
    /// </summary>
    public class PressKeyToRestart : MonoBehaviour
    {
        [SerializeField] KeyCode key = KeyCode.Space;

        void Update()
        {
            if (Input.GetKeyDown(key) || MicInput.isTriggered())
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}