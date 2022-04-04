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
