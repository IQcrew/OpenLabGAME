using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantKill : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag is "Destructible")
        {
            objectHP enemy = other.collider.GetComponent<objectHP>();
            enemy.TakeDamage(1000000);
        }
        else if (other.collider.tag is "Player")
        {
            Player pl = other.collider.GetComponent<Player>();
            pl.TakeDamage(10000);
        }
        else
        {

        }
        
    }
}
