using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    public GameObject ThisGameObject;
    public Sprite MedKit;
    private Sprite ActualItem;
    private bool IsMedKit = false;
    private int LengtOfDatabase;
    private int Timer= 0;
    private float StartTime;
    List<Gun> GunDatabase;
    private Sprite ObjTexture;
    private int TempRandom;
    private void Start(){
        ObjTexture = ThisGameObject.GetComponent<SpriteRenderer>().sprite;
        GameObject GM = GameObject.Find("LevelManager");
        List<Gun> GunDatabase = GM.GetComponent<GunManager>().AllGuns;
        LengtOfDatabase = GunDatabase.Count+1;
        Setup(); }
    private void Update()
    {
        //if (StartTime + Timer > Time.time) { ObjTexture = null; }
        //else if (StartTime + Timer + 1 > Time.time)
        //{
        //    if (IsMedKit) { ObjTexture = MedKit; }
        //    else { ObjTexture = ActualItem.GunTexture; }
        //    ThisGameObject.SetActive(true);
        //}
        //else if (StartTime + Timer + 15 > Time.time)
        //{

        //}
        //else if ( StartTime + Timer + 25 > Time.time)
        //{
        //    if (int.Parse(StartTime.ToString()) % 2 == 1)
        //    {
        //        if (IsMedKit) { ObjTexture = MedKit; }
        //        else { ObjTexture = ActualItem.GunTexture; }
        //    }
        //    else
        //    {
        //        ObjTexture = null;
        //    }
        //}
        //else { Setup(); }
        ThisGameObject.GetComponent<SpriteRenderer>().sprite = ActualItem;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (StartTime > Timer)
        {
            if (ActualItem.name != "" && collision.tag is "Player")
            {
                Player enemy = collision.GetComponent<Player>();
                if (enemy.PlayerName is "Player_1")
                {
                    if (Input.GetKeyDown(GlobalVariables.P1hit) && Input.GetKey(GlobalVariables.P1Down))
                    {
                        enemy.PickUpItem(ActualItem.name);
                    }
                }
                if (enemy.PlayerName is "Player_2")
                {
                    if (Input.GetKeyDown(GlobalVariables.P2hit) && Input.GetKey(GlobalVariables.P2Down))
                    {
                        enemy.PickUpItem(ActualItem.name);
                    }
                }
            }
        }
    }
    private void Setup()
    {
        Timer = Random.Range(5, 30);
        StartTime = Time.time;
        TempRandom = Random.Range(0,1100);
        if (TempRandom < 200) { IsMedKit = true;}
        else{
            IsMedKit = false;
            if (TempRandom < 400) { ActualItem = GunDatabase[1].ReturnTexture(); }
            else if (TempRandom < 550) { ActualItem = GunDatabase[2].ReturnTexture(); }
            else if (TempRandom < 700) { ActualItem = GunDatabase[3].ReturnTexture(); }
            else if (TempRandom < 800) { ActualItem = GunDatabase[4].ReturnTexture(); }
            else if (TempRandom < 900) { ActualItem = GunDatabase[5].ReturnTexture(); }
            else if (TempRandom < 950) { ActualItem = GunDatabase[6].ReturnTexture(); }
            else if (TempRandom < 1100) { ActualItem = GunDatabase[7].ReturnTexture(); }
        }
        ObjTexture = ActualItem;
    }
}
