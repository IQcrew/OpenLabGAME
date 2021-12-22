using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    private string ActualItem = "";
    private int LengtOfDatabase;
    private int Timer= 0;
    private float StartTime = Time.time;
    System.Random rrr = new System.Random();
    private void Start()
    {
        GameObject GM = GameObject.Find("LevelManager");
        List<Gun> GunDatabase = GM.GetComponent<GunManager>().AllGuns;
        LengtOfDatabase = GunDatabase.Count+1;

    }
    private void Update()
    {
           

    }







    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ActualItem != "" && collision.tag is "Player") {
            Player enemy = collision.GetComponent<Player>();
            if (enemy.PlayerName is "Player_1") {
                if (Input.GetKeyDown(GlobalVariables.P1hit) && Input.GetKey(GlobalVariables.P1Down)){
                    enemy.PickUpItem(ActualItem);
                }
            }
            if (enemy.PlayerName is "Player_2")
            {
                if (Input.GetKeyDown(GlobalVariables.P2hit) && Input.GetKey(GlobalVariables.P2Down))
                {
                    enemy.PickUpItem(ActualItem);
                }
            }
        }
    }
    private void OneLoop()
    {
    }


}
