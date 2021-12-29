using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    public GameObject ThisGameObject;
    public Sprite MedKit;
    private Gun ActualItem;
    private bool IsMedKit = false;
    private int LengtOfDatabase;
    private int Timer= 0;
    private float StartTime;
    private Sprite ObjTexture;
    private int TempRandom;
    private void Start(){
        ObjTexture = ThisGameObject.GetComponent<SpriteRenderer>().sprite;
        Setup(); }
    private void Update()
    {
        Debug.Log(ActualItem.GunTexture.ToString()+"  "+ObjTexture.ToString()+"  "+Timer.ToString()+ "  "+ Time.time.ToString(name));
        if (StartTime + Timer > Time.time) {}
        else if (StartTime + Timer + 1 > Time.time)
        {
            if (IsMedKit) { ObjTexture = MedKit; }
            else { ObjTexture = ActualItem.GunTexture; }
            ThisGameObject.SetActive(true);
        }
        else if (StartTime + Timer + 15 > Time.time)
        {

        }
        else if (StartTime + Timer + 25 > Time.time)
        {
            if (int.Parse(StartTime.ToString()) % 2 == 1)
            {
                if (IsMedKit) { ObjTexture = MedKit; }
                else { ObjTexture = ActualItem.GunTexture; }
            }
            else
            {
                ObjTexture = null;
            }
        }
        else { Setup(); }
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
        ThisGameObject.SetActive(false);
        ObjTexture = null;
        Timer = Random.Range(5, 30);
        StartTime = Time.time;
        TempRandom = Random.Range(0,1100);
        if (TempRandom < 200) { IsMedKit = true;}
        else{
            IsMedKit = false;
            if (TempRandom < 400) { ActualItem = GetGun("Pistol"); }
            else if (TempRandom < 550) { ActualItem = GetGun("Eagle"); }
            else if (TempRandom < 700) { ActualItem = GetGun("Mac-10"); }
            else if (TempRandom < 800) { ActualItem = GetGun("Pistol"); }
            else if (TempRandom < 900) { ActualItem = GetGun("SniperRifle"); }
            else if (TempRandom < 950) { ActualItem = GetGun("SniperRifle"); }
            else if (TempRandom < 1100) { ActualItem = GetGun("Shotgun"); }
        }
    }

    private Gun GetGun(string name)
    {
        GameObject GM = GameObject.Find("LevelManager");
        GunManager GunM = GM.GetComponent<GunManager>();
        foreach (var Gunitem in GunM.AllGuns)
        {
            if (name == Gunitem.name)
            {
                Gun TempGun = Gunitem.Clone();
                return TempGun;
            }
        }
        return GunM.AllGuns[1];
    }
}
