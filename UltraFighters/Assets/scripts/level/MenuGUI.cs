using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGUI : MonoBehaviour
{


    void Start()
    {
    }
    public void playOffline()
    {
        SceneManager.LoadScene("Demolition");
    }

}