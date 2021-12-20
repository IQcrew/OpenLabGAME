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
    public int damage;
    public double FireSpeed;
    public int BulletsOnShoot = 1;
    public GameObject P1_Bullet;
    public GameObject P2_Bullet;
    public Sprite GunTexture;
    public Sprite ShootingTexture;

    private double LastFire = -5f;

    public bool fire()
    {
        if (LastFire + FireSpeed < Time.time && ammo > 0)
        {
            LastFire = Time.time;
            ammo -= 1;
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
            damage = this.damage,
            FireSpeed = this.FireSpeed,
            BulletsOnShoot = this.BulletsOnShoot,
            P1_Bullet = this.P1_Bullet,
            P2_Bullet = this.P2_Bullet,
            GunTexture = this.GunTexture,
            ShootingTexture = this.ShootingTexture
        };
    }

}