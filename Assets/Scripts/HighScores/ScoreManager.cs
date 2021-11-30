using System;
using UnityEngine;

public class ScoreManager: MonoBehaviour
{
    [SerializeField]
    private static int highscore;

    [SerializeField] private static int score;


    private const string SCORE_KEY = "score";
    private const string HIGHSCORE_KEY = "highscore";
    
    private void Start()
    {
        score = GetStoredScore();
        highscore = GetStoredHighScore();
    }


    public static void SetScore(int s)
    {
        score = s;
        if (s > highscore)
            highscore = s;
    }


    public static void IncreaseScore()
    {
        SetScore(score+1);
    }

    
    public static int GetScore()
    {
        return score;
    }


    public static int GetHighscore()
    {
        return highscore;
    }

    public static void SaveScore()
    {
        PlayerPrefs.SetInt(SCORE_KEY, score);
        PlayerPrefs.SetInt(HIGHSCORE_KEY, highscore);
        PlayerPrefs.Save();
    }
    
    
    public static int GetStoredScore()
    {
        return _GetStoredScore(SCORE_KEY);
    }
    
    public static int GetStoredHighScore()
    {
        return _GetStoredScore(HIGHSCORE_KEY);
    }
    
    
    private static int _GetStoredScore(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetInt(key);
        else
            return 0;
    }
    

}