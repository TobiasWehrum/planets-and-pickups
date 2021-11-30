using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerInfo : MonoBehaviour
{
    private const string NAME_KEY = "name";

    private TMP_InputField InputField;



    public static void StoreName(string value)
    {
        PlayerPrefs.SetString(NAME_KEY, value);
        PlayerPrefs.Save();
    }

    public static string LoadName()
    {
        if (PlayerPrefs.HasKey(NAME_KEY))
            return PlayerPrefs.GetString(NAME_KEY);
        else
            return "Rando" + Random.Range(0,99).ToString("00");
    }
    
    

}