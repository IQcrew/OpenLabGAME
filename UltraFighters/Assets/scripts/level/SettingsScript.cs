using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        dataManager.readAllData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void backToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void changeKeyBind(string s)
    {
        KeyCode input = KeyCode.K;
        // execute("dataManager.gameData." + s + "=" + input);
    }
}
