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

    //keybinds Plyer1
    KeyCode rightP1 = KeyCode.RightArrow;
    KeyCode leftP1 = KeyCode.LeftArrow;
    KeyCode upP1 = KeyCode.UpArrow;
    KeyCode downP1 = KeyCode.DownArrow;
    KeyCode hitP1 = KeyCode.N;
    KeyCode fireP1 = KeyCode.M;
    KeyCode slotP1 = KeyCode.K;
    public KeyCode RightP1 { get { return rightP1; } set { rightP1 = value; dataManager.writeGame(); } }
    public KeyCode LeftP1 { get { return leftP1; } set { leftP1 = value; dataManager.writeGame(); } }
    public KeyCode UpP1 { get { return upP1; } set { upP1 = value; dataManager.writeGame(); } }
    public KeyCode DownP1 { get { return downP1; } set { downP1 = value; dataManager.writeGame(); } }
    public KeyCode HitP1 { get { return hitP1; } set { hitP1 = value; dataManager.writeGame(); } }
    public KeyCode FireP1 { get { return fireP1; } set { fireP1 = value; dataManager.writeGame(); } }
    public KeyCode SlotP1 { get { return slotP1; } set { slotP1 = value; dataManager.writeGame(); } }

    // keybinds Player2
    KeyCode rightP2 = KeyCode.D;
    KeyCode leftP2 = KeyCode.A;
    KeyCode upP2 = KeyCode.W;
    KeyCode downP2 = KeyCode.S;
    KeyCode hitP2 = KeyCode.X;
    KeyCode fireP2 = KeyCode.C;
    KeyCode slotP2 = KeyCode.V;
    public KeyCode RightP2 { get { return rightP2; } set { rightP2 = value; dataManager.writeGame(); } }
    public KeyCode LeftP2 { get { return leftP2; } set { leftP2 = value; dataManager.writeGame(); } }
    public KeyCode UpP2 { get { return upP2; } set { upP2 = value; dataManager.writeGame(); } }
    public KeyCode DownP2 { get { return downP2; } set { downP2 = value; dataManager.writeGame(); } }
    public KeyCode HitP2 { get { return hitP2; } set { hitP2 = value; dataManager.writeGame(); } }
    public KeyCode FireP2 { get { return fireP2; } set { fireP2 = value; dataManager.writeGame(); } }
    public KeyCode SlotP2 { get { return slotP2; } set { slotP2 = value; dataManager.writeGame(); } }
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
