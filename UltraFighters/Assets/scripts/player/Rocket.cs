using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] GameObject RocketEngine;
    [SerializeField] float speed;
    [SerializeField] int maxDamage;
    [SerializeField] public float radius = 3;
    [SerializeField] public float force;
    [SerializeField] public LayerMask LayerMask;
    [SerializeField] GameObject particle;
    [SerializeField] float particleDensity;
    [SerializeField] AudioClip EplosionSound;
    [SerializeField] [Range(0f, 1f)] float Volume = 1;
    private Rigidbody2D body;
    Transform EnginePos;

    void Start()
    {
        EnginePos = RocketEngine.GetComponent<Transform>();
        body = this.gameObject.GetComponent<Rigidbody2D>();
        body.velocity = transform.right * speed;
    }
    void Update()
    {
            Instantiate(particle, EnginePos.position ,EnginePos.rotation);
    }
    private void OnCollisionEnter2D(Collision2D other) //checkuje stretnutie z druhym objektom
    {


        if (other.collider.tag is "OneWayPlatform" || other.collider.tag is "Bullet")
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), other.collider);
            body.velocity = transform.right * speed;
            return;
        }
        else if (other.collider.tag is "OneTapBezZastavenia")
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), other.collider);
            objectHP enemy = other.collider.GetComponent<objectHP>();
            enemy.destroyObject();
            body.velocity = transform.right * speed;
            return;
        }
        else if (other.collider.tag is "Player")
        {
            

        }
        else
        {
            GameObject.Find("LevelManager").GetComponent<AudioSource>().PlayOneShot(EplosionSound, Volume);
            Explode(force, radius, LayerMask);
        }
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
