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

    public int xxxvolume = 100;
    public int Volume { get { return xxxvolume; } set { if (value <= 100 && value >= 0) { xxxvolume = value; dataManager.writeSettings(); } } }

    public Dictionary<string, KeyCode> KeyBinds = new Dictionary<string, KeyCode>()
    {
        {"P1 up", KeyCode.UpArrow },
        {"P1 down", KeyCode.DownArrow },
        {"P1 right", KeyCode.RightArrow},
        {"P1 left", KeyCode.LeftArrow},
        {"P1 hit", KeyCode.N},
        {"P1 fire", KeyCode.M },
        {"P1 slot", KeyCode.K },
        {"P2 up", KeyCode.W },
        {"P2 down", KeyCode.S },
        {"P2 right", KeyCode.D},
        {"P2 left", KeyCode.A},
        {"P2 hit", KeyCode.N},
        {"P2 fire", KeyCode.M },
        {"P2 slot", KeyCode.K }
    };
    public KeyCode getKeyBind(string key){
        try
        {
           return KeyBinds[key]; 
        }
        catch (System.Exception ex)
        {
            Debug.Log("keyBind not found: "+key);
            return KeyCode.F12;
        }
    }
    
    public void setKeyBind(string key, KeyCode value){
        KeyBinds[key] = value;
        dataManager.writeSettings();
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

