using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    public GameObject ThisGameObject;
    public Gun MedKit;
    private Gun ActualItem;
    private int Timer= 0;
    private float StartTime;
    private int TempRandom;
    private float TickTime = 0f;
    private void Start(){ Setup(); }
    private void Update()
    {

        if (StartTime + Timer > Time.time) {}  //waiting
        else if (StartTime + Timer + 1 > Time.time)
        { 
            ThisGameObject.GetComponent<SpriteRenderer>().sprite = ActualItem.GunTexture;
        }
        else if (StartTime + Timer + 15 > Time.time){ }
        else if (StartTime + Timer + 25 > Time.time)
        {
            if (Time.time-TickTime < 0.5f)
            {
                ThisGameObject.GetComponent<SpriteRenderer>().sprite = ActualItem.GunTexture;
            }
            else if (Time.time - TickTime < 1f) { ThisGameObject.GetComponent<SpriteRenderer>().sprite = null;}
            else{TickTime = Time.time; }
        }
        else {Setup(); }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (StartTime + Timer<Time.time)
        {
            if (ActualItem.name != "" && collision.tag == "Player")
            {
                Player enemy = collision.GetComponent<Player>();
                if (enemy.PickUpGun(ActualItem.name)) { Setup(); }
            }
        }
    }
    private void Setup()
    {
        ThisGameObject.GetComponent<SpriteRenderer>().sprite = null;
        Timer = Random.Range(5, 30);
        StartTime = Time.time;
        TempRandom = Random.Range(0,1100);
        if (TempRandom < 200) { GetGun("MedicKit"); }
        else if (TempRandom < 400) { ActualItem = GetGun("Pistol"); }
        else if (TempRandom < 550) { ActualItem = GetGun("Eagle"); }
        else if (TempRandom < 700) { ActualItem = GetGun("Mac-10"); }
        else if (TempRandom < 800) { ActualItem = GetGun("AssalutRifle"); }
        else if (TempRandom < 900) { ActualItem = GetGun("SniperRifle"); }
        else if (TempRandom < 950) { ActualItem = GetGun("AssalutRifle"); }
        else if (TempRandom < 1100) { ActualItem = GetGun("Shotgun"); }
        else { ActualItem = GetGun("MedicKit"); }
    }

    private Gun GetGun(string name)
    {
        GameObject GM = GameObject.Find("LevelManager");
        GunManager GunM = GM.GetComponent<GunManager>();
        if (name == "MedicKit") { return MedKit; }
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
