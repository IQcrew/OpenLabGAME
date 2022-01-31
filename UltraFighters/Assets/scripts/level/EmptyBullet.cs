using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBullet : MonoBehaviour
{
    GameObject ThisO;
    SpriteRenderer ThisS;
    float x = 1f;
    void Start()
    {
        ThisO = this.gameObject;
        ThisO.GetComponent<Rigidbody2D>().velocity = transform.right*(Random.Range(60,80)/10);
        ThisS = ThisO.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        x -= Time.deltaTime;
        if (x <= 0) { Destroy(ThisO); }
        ThisS.material.color = new Color(1f,1f,1f,x);
    }
}
