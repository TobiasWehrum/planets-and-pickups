using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIHandler : MonoBehaviour
{
    public TMP_Text highscoreText;
    
    public GameObject InputCanvas;

    public TMP_InputField inField;
    
    
    public CinemachineVirtualCamera MainCvm;
    public CinemachineVirtualCamera ScoreCvm;
    public CinemachineVirtualCamera HelpCvm;

    public GameObject mainTarget;
    public GameObject ScoreTarget;
    public GameObject HelpTarget;
    
    
    
    
    private string playerName;
    private int playerScore;

    private Highscores highscoreManager;
    
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        highscoreManager = FindObjectOfType<Highscores>();
        MoveCameraToMainCanvas();
        CloseInputPanel();
        playerName = PlayerInfo.LoadName();
        DisplayPlayerScore();
        SetTargets();
    }

    void SetTargets()
    {
        MainCvm.Follow = mainTarget.transform;
        ScoreCvm.Follow = ScoreTarget.transform;
        HelpCvm.Follow = HelpTarget.transform;
    }


    public void MoveCameraToMainCanvas()
    {
        MainCvm.Priority = 1;
        ScoreCvm.Priority = 0;
        HelpCvm.Priority = 0;
    }
    
    public void MoveCameraToHighscoreCanvas()
    {
        MainCvm.Priority = 0;
        ScoreCvm.Priority = 1;
        HelpCvm.Priority = 0;
    }
    
    public void MoveCameraToHelpCanvas()
    {
        MainCvm.Priority = 0;
        ScoreCvm.Priority = 0;
        HelpCvm.Priority = 1;
    }
    
    public void OpenInputPanel()
    {
        InputCanvas.SetActive(true);
    }
    
    void CloseInputPanel()
    {
        InputCanvas.SetActive(false);
    }

    public void StartGame()
    {
        ScoreManager.SetScore(0);
        ScoreManager.SaveScore();
        SceneManager.LoadScene("MainGame");
    }

    void DisplayPlayerScore()
    {
        playerScore = ScoreManager.GetHighscore();
        highscoreText.text = playerName +"'s score: " + playerScore.ToString("000");
    }
    

    public void SaveEditedName()
    {
        playerName = inField.text;
        PlayerInfo.StoreName(playerName);
        DisplayPlayerScore();
        CloseInputPanel();
    }

    public void SubmitHighscore()
    {
        Highscores.AddNewHighscore(playerName, playerScore);
        highscoreManager.DownloadHighscores();
    }


}
