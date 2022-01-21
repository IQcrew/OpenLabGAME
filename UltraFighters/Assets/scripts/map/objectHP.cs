using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectHP : MonoBehaviour
{
    public int health = 80;
    public AudioClip HitSound;
    public AudioClip DeathSound;
    private AudioSource AudioManagerSet;
    private void Start()
    {
        AudioManagerSet = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void TakeDamage (int damage)
    {
        health -= damage;

        if (health <= 1)     //DeathEffect and remove object
        {
            AudioManagerSet.PlayOneShot(DeathSound);
            Destroy(gameObject);
        }
        else      //HitEffect
        {
            AudioManagerSet.PlayOneShot(HitSound);
        }
    }
}
