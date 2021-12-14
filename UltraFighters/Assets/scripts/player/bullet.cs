using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 20;
    public Rigidbody2D RigidBodyObject;
    public float MaxBulletTime = 20f;
    private float StartTime = Time.time; 
    

    // Start is called before the first frame update
    void Start()
    {
        RigidBodyObject.velocity = transform.right * speed;
        StartTime = Time.time;
    }

    private void OnCollisionEnter2D (Collision2D other) //checkuje stretnutie z druhym objektom
    {
        if(other.collider.tag is "Destructible")
        {
            objectHP enemy = other.collider.GetComponent<objectHP>();
            enemy.TakeDamage(damage);
        }
        else if(other.collider.tag is "Player")
        {
            
        }

        Destroy(gameObject);
    }

    // Update is called once per frame

}
