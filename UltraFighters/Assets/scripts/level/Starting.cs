using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Starting : MonoBehaviour
{

    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        SceneManager.LoadScene("Menu");
    }

}
