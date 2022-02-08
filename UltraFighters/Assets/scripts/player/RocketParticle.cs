using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketParticle : MonoBehaviour
{
    SpriteRenderer sprite;
    Transform thisTransform;
    float x = 0.5f;
    float y = 0;
    void Start()
    {
        x = 0.5f; y = x;
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        thisTransform = this.gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        x = x - Time.deltaTime;
        if (x <= 0) { Destroy(this.gameObject); }
        sprite.color = new Color(1f, 1-x*2f, 0f,x);
        y =(0.5f-x)*5+1f;
        transform.localScale = new Vector3(y,y,y);
    }
}
