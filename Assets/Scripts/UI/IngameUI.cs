using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MiniPlanetDefense
{
    /// <summary>
    /// Provides access to the ingame UI.
    /// </summary>
    public class IngameUI : MonoBehaviour
    {
        [SerializeField] Text textScore;
        [SerializeField] GameObject introScreen;
        [SerializeField] GameObject restartScreen;
        [SerializeField] float restartScreenDelay = 1f;
        
        int score;

        void Awake()
        {
            introScreen.SetActive(true);
            restartScreen.SetActive(false);
        }

        public void SetScore(int value)
        {
            if (score == value)
                return;

            score = value;
            textScore.text = score.ToString();
        }

        public void ShowRestartScreen()
        {
            StartCoroutine(ShowRestartScreenCoroutine());
        }

        IEnumerator ShowRestartScreenCoroutine()
        {
            yield return new WaitForSeconds(restartScreenDelay);
            
            restartScreen.gameObject.SetActive(true);
        }
    }
}