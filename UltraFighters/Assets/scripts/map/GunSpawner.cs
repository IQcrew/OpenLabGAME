using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    public GameObject ThisGameObject;
    public Gun MedKit;
    private spawnTemplate ActualItem;
    private int Timer= 0;
    private float StartTime;
    private int TempRandom;
    private float TickTime = 0f;
    [SerializeField] private AudioSource AudioManager;
    [SerializeField] private AudioClip PickUpAudio;
    private void Start(){
        ActualItem = new spawnTemplate(MedKit);
        Setup(); 
        AudioManager.clip = PickUpAudio;
    }
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
            if (collision.tag == "Player")
            {
                Player enemy = collision.GetComponent<Player>();
                if (enemy.PickUpWeapon(ActualItem.name, ActualItem.type)) { AudioManager.PlayOneShot(ActualItem.ReloadPickup, ActualItem.reloadVolume); Setup(); }
            }
        }
    }
    private void Setup()
    {
        ThisGameObject.GetComponent<SpriteRenderer>().enabled = false;
        Timer = Random.Range(5, 30);
        StartTime = Time.time;
        TempRandom = Random.Range(0,1400);
        if (TempRandom < 200) { ActualItem.Generate("MedKit", "Gun"); }
        else if (TempRandom < 400) { ActualItem.Generate("Pistol", "Gun"); }
        else if (TempRandom < 550) { ActualItem.Generate("Eagle", "Gun"); }
        else if (TempRandom < 700) { ActualItem.Generate("Mac-10", "Gun"); }
        else if (TempRandom < 800) { ActualItem.Generate("AssalutRifle", "Gun"); }
        else if (TempRandom < 900) { ActualItem.Generate("SniperRifle", "Gun"); }
        else if (TempRandom < 950) { ActualItem.Generate("AssalutRifle", "Gun"); }
        else if (TempRandom < 1100) { ActualItem.Generate("Shotgun", "Gun"); }
        else if (TempRandom < 1200) { ActualItem.Generate("Axe", "Melee"); }
        else if (TempRandom < 1300) { ActualItem.Generate("Explosive", "Granade"); }
        else if (TempRandom < 1325) { ActualItem.Generate("MiniGun", "Gun"); }
        else if (TempRandom < 1400) { ActualItem.Generate("Axe", "Melee"); }
        else { ActualItem.Generate("MedKit", "Gun"); }
    }
    
    public class spawnTemplate
    {
        public string name;
        public string type;
        public Sprite weaponTexture;
        public AudioClip ReloadPickup;
        public float reloadVolume;
        public Gun SGun;
        public MeleeWeapon SMelee;
        public granadePack SGPack;

        private Gun MedKit;
        public spawnTemplate(Gun MedKitO){MedKit = MedKitO;}

        public void Generate(string name, string choosedType)
        {
            this.name = name; type = choosedType;
            GameObject GM = GameObject.Find("LevelManager");
            GunManager GunM = GM.GetComponent<GunManager>();
            switch (choosedType)
            {
                case "Gun":
                    foreach (var Gunitem in GunM.AllGuns)
                    {
                        if (name == Gunitem.name)
                        {
                            SGun = Gunitem.Clone();
                            weaponTexture = SGun.weaponTexture;
                            ReloadPickup = SGun.ReloadPickup;
                            reloadVolume = SGun.reloadVolume;
                            return;
                        }
                    }
                    //default MedKit
                    setMedKit(); break;
                case "Melee":
                    foreach(var meleeWeapon in GunM.AllMeleeWeapons)
            {
                        if (name == meleeWeapon.name)
                        {
                            SMelee = meleeWeapon;
                            weaponTexture = SMelee.weaponTexture;
                            ReloadPickup = SMelee.HitSound;
                            reloadVolume = SMelee.volume;
                            return;
                        }
                    }
                    setMedKit();break;
                case "Granade":
                    foreach (var granade in GunM.AllGranades)
                    {
                        if (name == granade.name)
                        {
                            SGPack = granade;
                            weaponTexture = SGPack.weaponTexture;
                            ReloadPickup = SGPack.PickUpAudio;
                            reloadVolume = SGPack.volume;
                            return;
                        }
                    }
                    setMedKit();break;
                default:
                    setMedKit();break;
            }
        }
        private void setMedKit()
        {
            type = "Gun";
            SGun = MedKit.Clone();
            name = SGun.name;
            weaponTexture = SGun.weaponTexture;
            ReloadPickup = SGun.ReloadPickup;
            reloadVolume = SGun.reloadVolume;
        }
    }
}
