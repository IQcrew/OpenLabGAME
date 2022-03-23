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
    public static void overWriteAllData(){writeGame(); writeSettings();}
}
public class dataSettings : MonoBehaviour
{
    private int volume = 100;
    public int Volume { get { return volume; } set { if (value <= 100 && value >= 0) { volume = value ; dataManager.writeSettings(); } } }

    ~dataSettings(){dataManager.overWriteAllData();}
}
public class dataGame : MonoBehaviour
{
    string lastScene;
    string lastMap;
    bool randomMap;

    public string LastScene { get { return lastScene; } set { lastScene = value; dataManager.writeGame(); } }
    public string LastMap { get { return lastMap; } set { lastMap = value; dataManager.writeGame(); } }
    public bool RandomMap { get { return randomMap; } set { randomMap = value; dataManager.writeGame(); } }



    ~dataGame(){dataManager.overWriteAllData();}
}
