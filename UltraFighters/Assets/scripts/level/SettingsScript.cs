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
    [SerializeField] private GameObject KeyBindsButtons; 
    void Start()
    {
        dataManager.readAllData();
        refreshButtonsText();
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
        dataManager.settingsData = new dataSettings();
        dataManager.writeSettings();
        refreshButtonsText();
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
    public void refreshButtonsText()
    {
        foreach (var key in dataManager.settingsData.xxxKeys)
        {
            try
            {
                KeyBindsButtons.transform.Find(key).GetComponentInChildren<TextMeshProUGUI>().text = dataManager.settingsData.getKeyBind(key).ToString();
            }
            catch { }
        }
    }
}
