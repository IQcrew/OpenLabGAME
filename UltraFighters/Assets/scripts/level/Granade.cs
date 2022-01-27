using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{

    [SerializeField] GameObject thisGameObject;
    [SerializeField] public float waitTime;
    [SerializeField] public float radius = 3;
    [SerializeField] public float force;
    [SerializeField] public float maxDamage = 100;
    [SerializeField] private LayerMask layerMask;

    private void Start()
    {
        StartCoroutine(TickOff());    
    }
    private IEnumerator TickOff()
    {
        yield return new WaitForSeconds(waitTime);
        Explode(force, radius, layerMask, thisGameObject);
    }
    public void Explode(float forceE, float radius, LayerMask layerMask, GameObject gameObject)
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
        Destroy(gameObject);
    }
}


