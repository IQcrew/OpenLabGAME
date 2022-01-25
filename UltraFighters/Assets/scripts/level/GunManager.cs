using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GunManager : MonoBehaviour
{
    public List<Gun> AllGuns = new List<Gun>();
    public List<MeleeWeapon> AllMeleeWeapons = new List<MeleeWeapon>();
    public List<Granades> AllGranades = new List<Granades>();
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
    public Sprite weaponTexture;
    public Sprite ShootingTextureP1;
    public Sprite ShootingTextureP2;
    public AudioClip Sound;
    [Range(0f,1f)]public float fireVolume = 1;
    public AudioClip ReloadPickup;
    [Range(0f, 1f)] public float reloadVolume = 1;

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
            weaponTexture = this.weaponTexture,
            ShootingTextureP1 = this.ShootingTextureP1,
            ShootingTextureP2 = this.ShootingTextureP2,
            Sound = this.Sound,
            ReloadPickup = this.ReloadPickup,
            reloadVolume = this.reloadVolume,
            fireVolume = this.fireVolume,
        };
    }
}

[System.Serializable]
public class MeleeWeapon
{
    public string name;
    public int damage;
    public int hitSpeed;
    public Sprite weaponTexture;
    public Animator PlayerAnimator;
    public AudioClip HitSound;
    [Range(0f, 1f)] public float volume = 1;

    public MeleeWeapon clone()
    {
        return new MeleeWeapon
        {
            name = this.name,
            damage = this.damage,
            hitSpeed = this.hitSpeed,
            weaponTexture = this.weaponTexture,
            PlayerAnimator = this.PlayerAnimator,
            HitSound = this.HitSound,
            volume = this.volume,
        };
    }
    
}
[System.Serializable]
public class Granades
{
    public string name;
    public Granade Grnd;
    public int numGranades;

    public Granades clone()
    {
        return new Granades
        {
            name = this.name,
            Grnd = this.Grnd,
            numGranades = this.numGranades,
        };
    }

}
