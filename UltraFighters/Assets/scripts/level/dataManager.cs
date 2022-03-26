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


public class dataSettings
{
    //  !!!!! you should never use xxx values !!!!!

    public int xxxvolume = 100;
    public int Volume { get { return xxxvolume; } set { if (value <= 100 && value >= 0) { xxxvolume = value ; dataManager.writeSettings(); } } }


    //keybinds Plyer1
    public KeyCode xxxrightP1 = KeyCode.RightArrow;
    public KeyCode xxxleftP1 = KeyCode.LeftArrow;
    public KeyCode xxxupP1 = KeyCode.UpArrow;
    public KeyCode xxxdownP1 = KeyCode.DownArrow;
    public KeyCode xxxhitP1 = KeyCode.N;
    public KeyCode xxxfireP1 = KeyCode.M;
    public KeyCode xxxslotP1 = KeyCode.K;
    public KeyCode RightP1 { get { return xxxrightP1; } set { xxxrightP1 = value; dataManager.writeGame(); } }
    public KeyCode LeftP1 { get { return xxxleftP1; } set { xxxleftP1 = value; dataManager.writeGame(); } }
    public KeyCode UpP1 { get { return xxxupP1; } set { xxxupP1 = value; dataManager.writeGame(); } }
    public KeyCode DownP1 { get { return xxxdownP1; } set { xxxdownP1 = value; dataManager.writeGame(); } }
    public KeyCode HitP1 { get { return xxxhitP1; } set { xxxhitP1 = value; dataManager.writeGame(); } }
    public KeyCode FireP1 { get { return xxxfireP1; } set { xxxfireP1 = value; dataManager.writeGame(); } }
    public KeyCode SlotP1 { get { return xxxslotP1; } set { xxxslotP1 = value; dataManager.writeGame(); } }


    // keybinds Player2
    public KeyCode xxxrightP2 = KeyCode.D;
    public KeyCode xxxleftP2 = KeyCode.A;
    public KeyCode xxxupP2 = KeyCode.W;
    public KeyCode xxxdownP2 = KeyCode.S;
    public KeyCode xxxhitP2 = KeyCode.X;
    public KeyCode xxxfireP2 = KeyCode.C;
    public KeyCode xxxslotP2 = KeyCode.V;
    public KeyCode RightP2 { get { return xxxrightP2; } set { xxxrightP2 = value; dataManager.writeGame(); } }
    public KeyCode LeftP2 { get { return xxxleftP2; } set { xxxleftP2 = value; dataManager.writeGame(); } }
    public KeyCode UpP2 { get { return xxxupP2; } set { xxxupP2 = value; dataManager.writeGame(); } }
    public KeyCode DownP2 { get { return xxxdownP2; } set { xxxdownP2 = value; dataManager.writeGame(); } }
    public KeyCode HitP2 { get { return xxxhitP2; } set { xxxhitP2 = value; dataManager.writeGame(); } }
    public KeyCode FireP2 { get { return xxxfireP2; } set { xxxfireP2 = value; dataManager.writeGame(); } }
    public KeyCode SlotP2 { get { return xxxslotP2; } set { xxxslotP2 = value; dataManager.writeGame(); } }
}


public class dataGame
{
    //  !!!!! you should never use xxx values !!!!!

    public string xxxlastScene;
    public string xxxlastMap;
    public bool xxxrandomMap;

    public string LastScene { get { return xxxlastScene; } set { xxxlastScene = value; dataManager.writeGame(); } }
    public string LastMap { get { return xxxlastMap; } set { xxxlastMap = value; dataManager.writeGame(); } }
    public bool RandomMap { get { return xxxrandomMap; } set { xxxrandomMap = value; dataManager.writeGame(); } }


}

