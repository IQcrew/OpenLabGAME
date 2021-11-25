using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MoveForce = 10;
    public float JumpForce = 3;     // Premenne na nastavenie pohybu - aby som sa nemusel furt chodit tam a spet po dokonceni fixnut a dat private

    public KeyCode Right;          // Nastavovanie pre klavesy pomocou unity
    public KeyCode Left;
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Slot1;
    public KeyCode Slot2;
    public KeyCode Slot3;

    private Vector2 Movement;

    bool isGrounded = false;
    bool Jumped = false;

    Rigidbody2D PlayerBody;

    // Start is called before the first frame update
    void Start()
    {
        PlayerBody = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGrounded)
        {
            if (Input.GetKey(Right))
            {
                PlayerBody.velocity = new Vector2(MoveForce, PlayerBody.velocity.y);
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (Input.GetKey(Left))
            {
                 PlayerBody.velocity = new Vector2(-MoveForce, PlayerBody.velocity.y);
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
                PlayerBody.velocity = new Vector2(0f, PlayerBody.velocity.y);
        }
        else if (Jumped)
        {
            if (Input.GetKey(Right))
            {
                PlayerBody.AddForce(new Vector2(MoveForce/200, 0f), ForceMode2D.Impulse);
                Jumped = true;
            }   
            else if (Input.GetKey(Left))
            {
                PlayerBody.AddForce(new Vector2(-MoveForce/200, 0f), ForceMode2D.Impulse);
                Jumped = true;
            }
        }

        if (Input.GetKey(Up) && isGrounded)
        {
            PlayerBody.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
             
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Jumped = false;
        }    
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Jumped = true;
        }
    }
}
