using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Gun
{
    //Options of gun
    public string name;
    public int ammo;
    public double FireSpeed;
    public GameObject Bullet;
    public GameObject GunTexture;
    public GameObject ShootingTexture;

    private double LastFire = -5f;

    public bool fire()
    {
        if (LastFire + FireSpeed < Time.time)
        {
            LastFire = Time.time;
            ammo -= 1;
            return true;
        }
        else{ return false; }
    }
}