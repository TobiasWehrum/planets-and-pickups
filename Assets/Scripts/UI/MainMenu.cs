using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject quitButton;
    private GameObject highScorePanel;
    
    
    



    void LoadGameScene()
    {
        SceneManager.LoadScene("MainGame");
    }

    void ShowHighScores()
    {
    }

    void SubmitHighScore()
    {
    }



}