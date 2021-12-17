using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GunManager : MonoBehaviour
{
    public Gun[] AllGuns = new Gun[2];
}

[System.Serializable]
public class Gun
{
    //Options of gun
    public string name;
    public int ammo;
    public int damage;
    public double FireSpeed;
    public GameObject Bullet;
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


}