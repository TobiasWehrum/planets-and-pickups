using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniPlanetDefense
{
    public class PressKeyToRestart : MonoBehaviour
    {
        [SerializeField] KeyCode key = KeyCode.Space;

        void Update()
        {
            if (Input.GetKeyDown(key))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}