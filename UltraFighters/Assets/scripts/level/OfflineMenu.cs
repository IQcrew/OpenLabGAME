using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineMenu : MonoBehaviour
{

    void Start()
    {
    }
    public void goToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
