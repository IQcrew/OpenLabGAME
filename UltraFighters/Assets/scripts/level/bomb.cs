using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public GameObject gmObject;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float radius;
    [SerializeField] private float force;
    

    void Start()
    {
        Explode(force, radius, layerMask, gmObject);
    }
    public void Explode(float force, float radius, LayerMask layerMask, GameObject gameObject)
    {
        
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        foreach (Collider2D obj in objects)
        {
            try
            {
                if (obj.tag == "Player")
                {
                    obj.GetComponent<Player>().giveExplosion();
                    Vector2 direction = obj.transform.position - transform.position;
                    obj.GetComponent<Rigidbody2D>().AddForce(direction * force / 1.2f, ForceMode2D.Impulse);
                }
                else
                {
                    Vector2 direction = obj.transform.position - transform.position;
                    obj.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
                }
            }
            catch { }
        }
        Destroy(gameObject);
    }
}
