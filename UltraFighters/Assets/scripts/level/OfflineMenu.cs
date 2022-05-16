using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class OfflineMenu : MonoBehaviour
{
    public List<string> maps = new List<string>();
    public List<string> skins  = new List<string>();
    public List<Sprite> skinImages = new List<Sprite> ();
    public List<Sprite> mapImages = new List<Sprite>();
    private int mapIndex = 0;
    private int p1Index = 0;
    private int p2Index = 0;
    public TextMeshProUGUI mapName;
    public Image p1Skin;
    public Image p2Skin;
    public Image mapImage;
    public TMP_InputField p1inputField;
    public TMP_InputField p2inputField;
    public GameObject playButton;
    private bool readyToPlay = false;
    void Start()
    {
        dataManager.readAllData();
        p1Index = skins.IndexOf(dataManager.gameData.SkinP1);
        p2Index = skins.IndexOf(dataManager.gameData.SkinP2);
        p1Skin.sprite = skinImages[p1Index];
        p2Skin.sprite = skinImages[p2Index];
        mapImage.sprite = mapImages[mapIndex];
        mapName.text = maps[mapIndex].ToUpper();
        dataManager.readAllData();
    }
    private void Update()
    {
        p1inputField.text = Regex.Replace(p1inputField.text, "[^a-zA-Z0-9_]", "");
        p2inputField.text = Regex.Replace(p2inputField.text, "[^a-zA-Z0-9_]", "");
        readyToPlay = p1inputField.text != p2inputField.text && p1inputField.text != "" && p2inputField.text != "";
        playButton.GetComponent<Image>().color = readyToPlay ? Color.white : Color.black;
    }
    public void goToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void play()
    {
        if (readyToPlay) { SceneManager.LoadScene(maps[mapIndex]); }
    }
    public void mapChangeRight()
    {
        if(mapIndex >= maps.Count - 1) 
            mapIndex = 0;
        else
            mapIndex++;
        mapImage.sprite = mapImages[mapIndex];
        mapName.text = maps[mapIndex].ToUpper();
    }
    public void mapChangeLeft()
    {
        if (mapIndex == 0)
            mapIndex = maps.Count - 1;
        else
            mapIndex--;
        mapImage.sprite = mapImages[mapIndex];
        mapName.text = maps[mapIndex].ToUpper();
    }
    public void skinPlayer1Right()
    {
        if (p1Index >= skins.Count - 1)
            p1Index = 0;
        else
            p1Index++;
        p1Skin.sprite = skinImages[p1Index];
        dataManager.gameData.SkinP1 = skins[p1Index];
    }
    public void skinPlayer1Left()
    {
        if (p1Index == 0)
            p1Index = skins.Count - 1;
        else
            p1Index--;
        p1Skin.sprite = skinImages[p1Index];
        dataManager.gameData.SkinP1 = skins[p1Index];
    }
    public void skinPlayer2Right()
    {
        if (p2Index >= skins.Count - 1)
            p2Index = 0;
        else
            p2Index++;
        p2Skin.sprite = skinImages[p2Index];
        dataManager.gameData.SkinP2 = skins[p2Index];
    }
    public void skinPlayer2Left()
    {
        if (p2Index == 0)
            p2Index = skins.Count - 1;
        else
            p2Index--;
        p2Skin.sprite = skinImages[p2Index];
        dataManager.gameData.SkinP2 = skins[p2Index];
    }
    public void nickChangeP1()
    {
        dataManager.gameData.NicknameP1 = p1inputField.text;
    }
    public void nickChangeP2()
    {
        dataManager.gameData.NicknameP2 = p2inputField.text;
    }

}
