using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SettingsScript : MonoBehaviour
{
    private GameObject currentObject;
    private Color32 normal = Color.white;
    private Color32 trigger = Color.yellow;
    private Color32 warning = Color.red;
    [SerializeField] private AudioClip clickSound;
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
        gameObject.GetComponent<AudioSource>().PlayOneShot(clickSound);
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
    public void changeSliderMV(GameObject parent)
    {
        dataManager.settingsData.masterVolume = parent.GetComponentInChildren<Slider>().value;
        parent.transform.Find("ValueMasterVolume").GetComponent<TextMeshProUGUI>().text = parent.GetComponentInChildren<Slider>().value.ToString();
    }
    public void changeSliderSE(GameObject parent)
    {
        dataManager.settingsData.soundEffectsVolume = parent.GetComponentInChildren<Slider>().value;
        parent.transform.Find("ValueSoundEffects").GetComponent<TextMeshProUGUI>().text = parent.GetComponentInChildren<Slider>().value.ToString();

    }
    public void changeSliderMU(GameObject parent)
    {
        dataManager.settingsData.musicVolume = parent.GetComponentInChildren<Slider>().value;
        parent.transform.Find("ValueMusic").GetComponent<TextMeshProUGUI>().text = parent.GetComponentInChildren<Slider>().value.ToString();
    }
    public void fullScreen(TextMeshProUGUI text)
    {
        Screen.fullScreen = text.text == "Full-Screen";
    }
    public void changeResolution(TextMeshProUGUI text)
    {
        Dictionary<string, int[]> resolutions = new Dictionary<string, int[]>()
        {
            {"Full HD" , new int[] {1920,1080} },
            {"HD", new int[] {1280,720}},
            {"480p", new int[] {854,480} }
        };
        Screen.SetResolution(resolutions[text.ToString()][0], resolutions[text.ToString()][1], Screen.fullScreen);
    }
}
