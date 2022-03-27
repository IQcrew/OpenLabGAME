using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OfflineMenu : MonoBehaviour
{
    public List<string> maps = new List<string>();
    public List<Sprite> skins = new List<Sprite> ();
    public List<Sprite> mapImages = new List<Sprite>();
    private int mapIndex = 0;
    private int p1Index = 0;
    private int p2Index = 0;
    public TextMeshProUGUI mapName;
    public Image p1Skin;
    public Image p2Skin;
    public Image mapImage;
    void Start()
    {
        p1Skin.sprite = skins[p1Index];
        p2Skin.sprite = skins[p2Index];
        mapImage.sprite = mapImages[mapIndex];
        mapName.text = maps[mapIndex].ToUpper();
    }
    public void goToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void play()
    {
        SceneManager.LoadScene(maps[mapIndex]);
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
        p1Skin.sprite = skins[p1Index];
    }
    public void skinPlayer1Left()
    {
        if (p1Index == 0)
            p1Index = skins.Count - 1;
        else
            p1Index--;
        p1Skin.sprite = skins[p1Index];
    }
    public void skinPlayer2Right()
    {
        if (p2Index >= skins.Count - 1)
            p2Index = 0;
        else
            p2Index++;
        p2Skin.sprite = skins[p2Index];
    }
    public void skinPlayer2Left()
    {
        if (p2Index == 0)
            p2Index = skins.Count - 1;
        else
            p2Index--;
        p2Skin.sprite = skins[p2Index];
    }

}
