using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public GameObject gmObject;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float radius;
    [SerializeField] private float force;
    void Update()
    {
        if (Input.GetKey(KeyCode.N)) { Explode(force,radius,layerMask,gmObject);  }
    }
    public void Explode(float force, float radius, LayerMask layerMask, GameObject gameObject)
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;
            obj.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        }
        Destroy(gameObject);
    }
}
