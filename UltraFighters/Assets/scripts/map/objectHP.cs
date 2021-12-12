using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectHP : MonoBehaviour
{
    public int health = 80;
    public GameObject HitEffect;
    public GameObject DeathEffect;



    // Update is called once per frame
    public void TakeDamage (int damage)
    {
        health -= damage;

        if (health <= 1)     //DeathEffect and remove object
        {

            Destroy(gameObject);
        }
        else      //HitEffect
        {     
            
        }
    }
}
