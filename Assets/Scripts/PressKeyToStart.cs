using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniPlanetDefense
{
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
            if (Input.GetKeyDown(key))
            {
                gameObject.SetActive(false);
            }
        }
    }
}