using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;
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

    Rigidbody2D PlayerBody;
    BoxCollider2D PlayerHitBox;
    SpriteRenderer PlayerRender;

    // Start is called before the first frame update
    void Start()
    {
        PlayerBody = GetComponent<Rigidbody2D>();
        PlayerHitBox = GetComponent<BoxCollider2D>();
        PlayerRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }
    private void Move()
    {
        if(Input.GetKey(Right) && !Input.GetKey(Left))
        {
            PlayerBody.velocity = new Vector2(+MoveForce, PlayerBody.velocity.y);
            PlayerRender.flipX = false;
        }
        else if (Input.GetKey(Left) && !Input.GetKey(Right))
        {
            PlayerBody.velocity = new Vector2(-MoveForce, PlayerBody.velocity.y);
            PlayerRender.flipX = true;
        }
        else
            PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y);

        

            
    }
    private void Jump()
    {
        if(Input.GetKey(Up) && isGrounded())
        {
            PlayerBody.velocity = Vector2.up * JumpForce;
        }
    }    
    private bool isGrounded()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(PlayerHitBox.bounds.center, PlayerHitBox.bounds.size, 0f, Vector2.down, 0.01f, platformLayerMask);
            return rayCastHit.collider != null;
    }
}
