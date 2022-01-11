using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GunManager : MonoBehaviour
{
    public List<Gun> AllGuns = new List<Gun>();

}

[System.Serializable]
public class Gun
{
    //Options of gun
    public string name;
    public int ammo;
    public int speed = 50;
    public int damage;
    public double FireSpeed;
    public int BulletsOnShoot = 0;
    public GameObject Bullet;
    public GameObject Bullet2P;
    public Sprite GunTexture;
    public Sprite ShootingTexture;

    private double LastFire = -5f;

    public bool fire()
    {
        if (LastFire + FireSpeed < Time.time && ammo > 0)
        {
            LastFire = Time.time;
            return true;
        }
        else { return false; }
    }
    public Gun Clone()
    {
        return new Gun
        {
            name = this.name,
            ammo = this.ammo,
            speed = this.speed,
            damage = this.damage,
            FireSpeed = this.FireSpeed,
            BulletsOnShoot = this.BulletsOnShoot,
            Bullet = this.Bullet,
            Bullet2P = this.Bullet2P,
            GunTexture = this.GunTexture,
            ShootingTexture = this.ShootingTexture
        };
    }
    public Sprite ReturnTexture(){ return GunTexture; }

}