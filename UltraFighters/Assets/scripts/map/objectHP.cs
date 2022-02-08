using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectHP : MonoBehaviour
{
    public int health = 80;
    public AudioClip HitSound;
    [Range(0f, 1f)] public float hitVolume = 1;
    public AudioClip DeathSound;
    [Range(0f, 1f)] public float deathVolume = 1;
    private AudioSource AudioManagerSet;
    private void Start()
    {
        AudioManagerSet = GameObject.Find("LevelManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void TakeDamage (int damage)
    {
        health -= damage;

        if (health <= 1)     //DeathEffect and remove object
        {
            AudioManagerSet.PlayOneShot(DeathSound,deathVolume);
        }
        else      //HitEffect
        {
            AudioManagerSet.PlayOneShot(HitSound, hitVolume);
        }
    }
    public void destroyObject()
    {
        Destroy(gameObject);
    }
}
