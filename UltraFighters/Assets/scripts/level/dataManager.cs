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
         overWriteAllData();
    }
    public static void updateAllData()
    {
        string json = File.ReadAllText(settingsPath);
        settingsData = JsonUtility.FromJson<dataSettings>(json);
        json = File.ReadAllText(gamePath);
        gameData = JsonUtility.FromJson<dataGame>(json);
    }
    public static void overWriteAllData()
    {
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(gamePath, json);
        json = JsonUtility.ToJson(settingsData);
        File.WriteAllText(settingsPath, json);
    }
}
public class dataSettings : MonoBehaviour
{
    public int volume = 100;


    ~dataSettings()
    {
        dataManager.overWriteAllData();
    }
}
public class dataGame : MonoBehaviour
{
    public string lastScene;
    public string lastMap;
    public bool randomMap;



    ~dataGame()
    {
        dataManager.overWriteAllData();
    }
}
