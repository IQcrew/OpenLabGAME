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
    [SerializeField] private AudioSource AudioManager;
    [SerializeField] private AudioClip PickUpAudio;
    [Header("Granades")]

    private SpriteRenderer ThisRender;
    private void Start(){ Setup(); AudioManager.clip = PickUpAudio; ThisRender = ThisGameObject.GetComponent<SpriteRenderer>(); }
    private void Update()
    {

        if (StartTime + Timer > Time.time) {}  //waiting
        else if (StartTime + Timer + 1 > Time.time)
        {
            ThisGameObject.GetComponent<SpriteRenderer>().enabled = true;
            ThisGameObject.GetComponent<SpriteRenderer>().sprite = ActualItem.weaponTexture;
        }
        else if (StartTime + Timer + 15 > Time.time){ }
        else if (StartTime + Timer + 25 > Time.time)
        {
            if (Time.time-TickTime < 0.5f)
            {
                ThisGameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (Time.time - TickTime < 1f) { ThisGameObject.GetComponent<SpriteRenderer>().enabled = false;}
            else{TickTime = Time.time; }
        }
        else {Setup(); }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (StartTime + Timer < Time.time)
        {
            if (ActualItem.name != "" && collision.tag == "Player")
            {
                Player enemy = collision.GetComponent<Player>();
                if (enemy.PickUpGun(ActualItem.name)) { AudioManager.PlayOneShot(ActualItem.ReloadPickup, ActualItem.reloadVolume); Setup(); }
                
            }
        }
        
    }
    private void Setup()
    {
        ThisGameObject.GetComponent<SpriteRenderer>().enabled = false;
        Timer = Random.Range(5, 30);
        StartTime = Time.time;
        TempRandom = Random.Range(0,1100);
        if (TempRandom < 200) { ActualItem = MedKit; }
        else if (TempRandom < 400) { ActualItem = GetGun("Pistol"); }
        else if (TempRandom < 550) { ActualItem = GetGun("Eagle"); }
        else if (TempRandom < 700) { ActualItem = GetGun("Mac-10"); }
        else if (TempRandom < 800) { ActualItem = GetGun("AssalutRifle"); }
        else if (TempRandom < 900) { ActualItem = GetGun("SniperRifle"); }
        else if (TempRandom < 950) { ActualItem = GetGun("AssalutRifle"); }
        else if (TempRandom < 1100) { ActualItem = GetGun("Shotgun"); }
        else { ActualItem = MedKit;}
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
