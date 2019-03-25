using UnityEngine;
using UnityEngine.UI;

namespace MiniPlanetDefense
{
    public class IngameUI : MonoBehaviour
    {
        [SerializeField] Text textScore;
        
        int score;

        public void SetScore(int value)
        {
            if (score == value)
                return;

            score = value;
            textScore.text = score.ToString();
        }
    }
}