using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] int maxDamage;
    [SerializeField] float Speed;
    [SerializeField] GameObject particle;
    [SerializeField] AudioClip EplosionSound;
    [SerializeField] [Range(0f, 1f)] float fireVolume = 1;


    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void Explode(float forceE, float radius, LayerMask layerMask)
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        foreach (Collider2D obj in objects)
        {
            try
            {
                float FractionDamge = 1 / (radius / Vector3.Distance(obj.GetComponent<Transform>().position, transform.position));
                if (obj.tag == "Player")
                {
                    obj.GetComponent<Player>().giveExplosion((int)(maxDamage * FractionDamge));
                    Vector2 direction = obj.transform.position - transform.position;
                    obj.GetComponent<Rigidbody2D>().AddForce(direction * forceE / 1.2f, ForceMode2D.Impulse);
                }
                else
                {
                    Vector2 direction = obj.transform.position - transform.position;
                    obj.GetComponent<Rigidbody2D>().AddForce(direction * forceE, ForceMode2D.Impulse);
                }
            }
            catch { }
        }
        Destroy(this.gameObject);
    }
}
