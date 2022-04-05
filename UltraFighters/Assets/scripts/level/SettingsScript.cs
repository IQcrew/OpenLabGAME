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
    private Color32 warning = Color.red;
    [SerializeField] private GameObject KeyBindsButtons;
    [SerializeField] private TextMeshProUGUI warningBox;
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
            if(currentObject.GetComponentInChildren<TextMeshProUGUI>().text == "")
                currentObject.GetComponent<Image>().color = warning;
            else
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
                if (dataManager.settingsData.KeyCodeValues.Contains(e.keyCode))
                {
                    int index = dataManager.settingsData.KeyCodeValues.IndexOf(e.keyCode);
                    dataManager.settingsData.KeyCodeValues[index] = KeyCode.None;
                    KeyBindsButtons.transform.Find(dataManager.settingsData.xxxKeys[index]).GetComponent<Image>().color = warning;
                    KeyBindsButtons.transform.Find(dataManager.settingsData.xxxKeys[index]).GetComponentInChildren<TextMeshProUGUI>().text = "";

                }
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
