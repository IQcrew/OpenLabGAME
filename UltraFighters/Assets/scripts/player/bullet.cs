using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class bullet : MonoBehaviour
{
    public string shooter_name = "";
    public float speed = 20f;
    public int damage = 20;
    public Rigidbody2D RigidBodyObject;
    public float MaxBulletTime = 20f;
    

    // Start is called before the first frame update
    void Start()
    {
        RigidBodyObject.velocity = transform.right * speed;
        
    }


    private void OnCollisionEnter2D (Collision2D other) //checkuje stretnutie z druhym objektom
    {
        
        if(other.collider.tag is "Destructible")
        {
            objectHP enemy = other.collider.GetComponent<objectHP>();
            enemy.TakeDamage(damage);
        }
        else if (other.collider.tag is "OneWayPlatform" || other.collider.tag is "Bullet")
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), other.collider);
            RigidBodyObject.velocity = transform.right * speed;
            return;
        }
        else if (other.collider.tag is "OneTapBezZastavenia")
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), other.collider);
            objectHP enemy = other.collider.GetComponent<objectHP>();
            enemy.TakeDamage(1000);
            RigidBodyObject.velocity = transform.right * speed;
            return;
        }
        else if(other.collider.tag is "Player")
        {
            if (other.collider.name != shooter_name)
            {
                objectHP enemyP = other.collider.GetComponent<objectHP>();
                enemyP.TakeDamage(damage);
            }
            else
            {
                return;
            }

        }

        Destroy(gameObject);
    }


    // Update is called once per frame

}
