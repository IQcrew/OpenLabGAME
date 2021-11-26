using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private float MoveForce = 5f;
    [SerializeField] private float JumpForce = 5f;
    [SerializeField] private float DoubleTapTime = 0.2f;
    private float LastClickTime = 0;
    public KeyCode Right;          // Nastavovanie pre klavesy pomocou unity
    public KeyCode Left;
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Slot1;
    public KeyCode Slot2;
    public KeyCode Slot3;

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
        float TimeSinceLastClick = Time.time - LastClickTime;
        if (DoubleTapTime >= TimeSinceLastClick)
        {
            if (Input.GetKey(Right) && !Input.GetKey(Left))
            {
                PlayerBody.velocity = new Vector2(+MoveForce * 2, PlayerBody.velocity.y);
                PlayerRender.flipX = false;
            }
            else if (Input.GetKey(Left) && !Input.GetKey(Right))
            {
                PlayerBody.velocity = new Vector2(-MoveForce * 2, PlayerBody.velocity.y);
                PlayerRender.flipX = true;
            }
            else
                PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y);
        }
        else
        {
            if (Input.GetKey(Right) && !Input.GetKey(Left))
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
        RaycastHit2D rayCastHit = Physics2D.BoxCast(PlayerHitBox.bounds.center, PlayerHitBox.bounds.size, 0f, Vector2.down, 0.02f, platformLayerMask);
            return rayCastHit.collider != null;
    }    
}

