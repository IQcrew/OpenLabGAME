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
    private void Start()
    {
        textObject.SetActive(false);
        StartCoroutine(LateStart(0.01f));
    }
    
     

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        System.Random rrr = new System.Random();
        for (int i = 0; i < PlayersInGame.Count; i++)
        {
            int temp = rrr.Next(PlayerSpawns.Count);
            PlayersInGame[i].GetComponent<Transform>().position = new Vector3(PlayerSpawns[temp].GetComponent<Transform>().position.x,PlayerSpawns[temp].GetComponent<Transform>().position.y, 0f);
            
            PlayerSpawns.RemoveAt(temp);
        }
    }
private void Update()
    {
        if (endScreen)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
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
