using UnityEngine;
using System.Collections;

public class Highscores : MonoBehaviour {
    
    // http://dreamlo.com/lb/Q8ED0bIu8UiTz5U7BuV9ywiThpS36I1UmxKQE0K2BCDA
    const string privateCode = "Q8ED0bIu8UiTz5U7BuV9ywiThpS36I1UmxKQE0K2BCDA";
    const string publicCode = "61a358d88f40bb127889a895";
    const string webURL = "http://dreamlo.com/lb/";

    DisplayHighscores highscoreDisplay;
    public Highscore[] highscoresList;
    static Highscores instance;


    void DebugScores()
    {
        AddNewHighscore("Avi", 2);
        AddNewHighscore("Sam", 20);
        AddNewHighscore("Bob Ross", 30);
    }

    void Awake() {
        
        highscoreDisplay = GetComponent<DisplayHighscores> ();
        instance = this;
    }

    public static void AddNewHighscore(string username, int score) {
        instance.StartCoroutine(instance.UploadNewHighscore(username,score));
    }

    IEnumerator UploadNewHighscore(string username, int score) {
        if ((score > 0) && (username.Length>0))
        {
            WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
            yield return www;

            if (string.IsNullOrEmpty(www.error)) {
                print ("Upload Successful");
                DownloadHighscores();
            }
            else {
                print ("Error uploading: " + www.error);
            }
        }
    }

    public void DownloadHighscores() {
        StartCoroutine("DownloadHighscoresFromDatabase");
    }

    IEnumerator DownloadHighscoresFromDatabase() {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;
		
        if (string.IsNullOrEmpty (www.error)) {
            FormatHighscores (www.text);
            highscoreDisplay.OnHighscoresDownloaded(highscoresList);
        }
        else {
            print ("Error Downloading: " + www.error);
        }
    }

    void FormatHighscores(string textStream) {
        string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
        highscoresList = new Highscore[entries.Length];

        for (int i = 0; i <entries.Length; i ++) {
            string[] entryInfo = entries[i].Split(new char[] {'|'});
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            highscoresList[i] = new Highscore(username,score);
            print (highscoresList[i].username + ": " + highscoresList[i].score);
        }
    }

}

public struct Highscore {
    public string username;
    public int score;

    public Highscore(string _username, int _score) {
        username = _username;
        score = _score;
    }

}