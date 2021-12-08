using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D RigidBodyObject;
    public float MaxBulletTime = 20f;
    private float StartTime = Time.time;

    // Start is called before the first frame update
    void Start()
    {
        RigidBodyObject.velocity = transform.right * speed;
        StartTime = Time.time;
    }

    private void Update()
    {
        if (StartTime + MaxBulletTime < Time.time) { Destroy(gameObject); }
    }

    // Update is called once per frame

}
