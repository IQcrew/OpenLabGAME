using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    private GameObject currentObject;
    private Color32 normal = Color.white;
    private Color32 trigger = Color.yellow;
    void Start()
    {
        dataManager.readAllData();
    }
    public void backToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void changeKeyBind(GameObject self)
    {
        if (currentObject != null)
            currentObject.GetComponent<Image>().color = normal;
        currentObject = self;
        currentObject.GetComponent<Image>().color = trigger;
    }
    public void loadDefault()
    {
        dataManager.settingsData.xxxKeyCodeValues = new List<KeyCode>(){
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
        KeyCode.N, // P2 hit
        KeyCode.M, // P2 fire
        KeyCode.K, // P2 slot
        };
        dataManager.writeSettings();
    }
    private void OnGUI()
    {
        if (currentObject != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                dataManager.settingsData.setKeyBind(currentObject.name, e.keyCode);
                currentObject.GetComponentInChildren<TextMeshProUGUI>().text = e.keyCode.ToString();
                currentObject.GetComponent<Image>().color = normal;
                currentObject = null;
            }
        }
    }
}
