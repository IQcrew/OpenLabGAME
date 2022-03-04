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
    public List<GameObject> PlayersAlive = new List<GameObject>();
    public List<GameObject> PlayersInGame = new List<GameObject>();

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
    public void appendPlayer(GameObject player){ PlayersInGame.Add(player); PlayersAlive.Add(player); }

    public void PlayerDeath(GameObject player)
    {
        PlayersAlive.Remove(player);
        if( PlayersAlive.Count == 1) {
            TextBox.text = PlayersAlive[0].name;
            endScreen = true; textObject.SetActive(true);
        }
        else if(PlayersAlive.Count < 1){
            TextBox.text = "remiza";
            endScreen = true; textObject.SetActive(true);
        }
    }
}
