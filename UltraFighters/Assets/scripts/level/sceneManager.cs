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
    [System.NonSerialized] public List<GameObject> PlayersAlive = new List<GameObject>();
    [System.NonSerialized] public List<GameObject> PlayersInGame = new List<GameObject>();
    public List<GameObject> PlayerSpawns = new List<GameObject>();
    [System.NonSerialized] public bool Paused = false;
    AudioListener audioListener;
    [SerializeField] GameObject menuScreen;
    public GameObject HudP1;
    public GameObject HudP2;
    private void Start()
    {
        audioListener = GameObject.Find("Camera").GetComponent<AudioListener>();
        textObject.SetActive(false);
        StartCoroutine(LateStart(0.01f));
        menuScreen.SetActive(false);
    }
    public void setTexture(string playerName, string nameOfTexture, Sprite sourceIMG)
    {
        GameObject temp = playerName == "Player_1" ? HudP1 : HudP2;
        Image image = temp.transform.Find("Gun").GetComponent<Image>();
        image.color =  sourceIMG != null ? Color.white : Color.clear;
        image.sprite = sourceIMG;
    }
    public void setHP(string playerName,int value)
    {
        GameObject temp = playerName == "Player_1" ? HudP1 : HudP2;
    }
    public void setName(string playerName, string text)
    {
        GameObject temp = playerName == "Player_1" ? HudP1 : HudP2;
    }
     

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        System.Random rrr = new System.Random();
        for (int i = 0; i < PlayersInGame.Count; i++)
        {
            int temp = rrr.Next(PlayerSpawns.Count);
            PlayersInGame[i].GetComponent<Transform>().position = new Vector3(PlayerSpawns[temp].GetComponent<Transform>().position.x,PlayerSpawns[temp].GetComponent<Transform>().position.y, 0f);
            PlayersInGame[i].GetComponent<Player>().PlayerRotationRight = rrr.Next(2) == 0;
            PlayerSpawns.RemoveAt(temp);
        }
    }
private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { changePauseStatus(); }
        if (endScreen)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    public void changePauseStatus()
    {
        if (Paused)
        {
            Paused = false;
            audioListener.enabled = true;
            menuScreen.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Paused = true;
            audioListener.enabled = false;
            menuScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void backToMainMenu(){ SceneManager.LoadScene("Menu"); }
    public void appendPlayer(GameObject player){ PlayersInGame.Add(player); PlayersAlive.Add(player); }

    public void PlayerDeath(GameObject player)
    {
        PlayersAlive.Remove(player);
        if( PlayersAlive.Count == 1) {
            TextBox.text = PlayersAlive[0].GetComponent<Player>().PlayerName;
            endScreen = true; textObject.SetActive(true);
        }
        else if(PlayersAlive.Count < 1){
            TextBox.text = "remiza";
            endScreen = true; textObject.SetActive(true);
        }
    }
}
