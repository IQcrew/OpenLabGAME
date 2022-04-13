using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class dataManager
{
    private static string gamePath = "Assets/data/game.json";
    private static string settingsPath = "Assets/data/settings.json";

    public static dataGame gameData = new dataGame();
    public static dataSettings settingsData = new dataSettings();
    static dataManager()
    {
        readAllData();
    }
    public static void readAllData()
    {
        string json = File.ReadAllText(settingsPath);
        settingsData = JsonUtility.FromJson<dataSettings>(json);
        json = File.ReadAllText(gamePath);
        gameData = JsonUtility.FromJson<dataGame>(json);
    }
    public static void writeGame()
    {
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(gamePath, json);
    }
    public static void writeSettings()
    {
        string json = JsonUtility.ToJson(settingsData);
        File.WriteAllText(settingsPath, json);
    }
}


public class dataSettings
{
    //  !!!!! you should never use xxx values !!!!!

    public float xxxMasterVolume = 100;
    public float masterVolume { get { return xxxMasterVolume; } set { if (value <= 100 && value >= 0) { xxxMasterVolume = value; dataManager.writeSettings(); } } }
    public float xxxMusicVolume = 100;
    public float musicVolume { get { return xxxMusicVolume; } set { if (value <= 100 && value >= 0) { xxxMusicVolume = value; dataManager.writeSettings(); } } }
    public float xxxSoundEffectsVolume = 100;
    public float soundEffectsVolume { get { return xxxSoundEffectsVolume; } set { if (value <= 100 && value >= 0) { xxxSoundEffectsVolume = value; dataManager.writeSettings(); } } }

    // keyBinds
    public List<string> xxxKeys = new List<string>()
    {
        "P1 up",
        "P1 down",
        "P1 right",
        "P1 left",
        "P1 hit",
        "P1 fire",
        "P1 slot",
        "P2 up",
        "P2 down",
        "P2 right",
        "P2 left",
        "P2 hit",
        "P2 fire",
        "P2 slot",
    };

    public List<KeyCode> KeyCodeValues = new List<KeyCode>()
    {
    };

    public KeyCode getKeyBind(string key){
        try
        {
            if (KeyCodeValues.Contains(KeyCode.None)) { throw new System.Exception(); }
            return KeyCodeValues[xxxKeys.IndexOf(key)];
        }
        catch
        {
            Debug.Log("keyBind not found: "+key);
            KeyCodeValues = new List<KeyCode>()
            {
                KeyCode.UpArrow, // P1 up
                KeyCode.DownArrow, // P1 down
                KeyCode.RightArrow, // P1 right
                KeyCode.LeftArrow, // P1 left
                KeyCode.N, // P1 hit
                KeyCode.M, // P1 fire
                KeyCode.K, // P1 slot
                KeyCode.W, // P2 up
                KeyCode.S, // P2 down
                KeyCode.D, // P2 right
                KeyCode.A, // P2 left
                KeyCode.X, // P2 hit
                KeyCode.C, // P2 fire
                KeyCode.V, // P2 slot
            };
            dataManager.writeSettings();
            return KeyCodeValues[xxxKeys.IndexOf(key)];
        }
      
    }
    
    public void setKeyBind(string key, KeyCode value){
        try
        {
            KeyCodeValues[xxxKeys.IndexOf(key)] = value;
            dataManager.writeSettings();
        }
        catch
        {
            Debug.Log("keyBind index not found: " + key);
        }
    }

}


public class dataGame
{
    //  !!!!! you should never use xxx values !!!!!

    public string xxxlastScene;
    public string xxxlastMap;
    public bool xxxrandomMap;
    public string xxxNicknameP1;
    public string xxxNicknameP2;

    public string LastScene { get { return xxxlastScene; } set { xxxlastScene = value; dataManager.writeGame(); } }
    public string LastMap { get { return xxxlastMap; } set { xxxlastMap = value; dataManager.writeGame(); } }
    public bool RandomMap { get { return xxxrandomMap; } set { xxxrandomMap = value; dataManager.writeGame(); } }
    public string NicknameP1 { get { return xxxNicknameP1; } set { xxxNicknameP1 = value; dataManager.writeGame(); } }
    public string NicknameP2 { get { return xxxNicknameP2; } set { xxxNicknameP2 = value; dataManager.writeGame(); } }


}

