using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sceneManager : MonoBehaviour
{
    private bool endScreen = false;
    [SerializeField] private Text TextBox;
    [SerializeField] private GameObject textObject;


    private void Start()
    {
        textObject.SetActive(false);
    }
    private void Update()
    {
        if (endScreen)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                endScreen = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    public void setEndScreen(string loser) {
        TextBox.text = loser == "Player_1" ? "Player2 Wins": "Player1 Wins";
        endScreen = true; textObject.SetActive(true);
    
    }



}
