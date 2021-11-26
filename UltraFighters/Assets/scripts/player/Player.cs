using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private float WalkForce = 5f;
    [SerializeField] private float SprintForce = 8f;
    [SerializeField] private float JumpForce = 5f;
    [SerializeField] private float DoubleTapTime = 0.2f;
    private float LastClickTime = Time.time;
    private bool sprinting = false;
    private bool LastHoldedKey = false;
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
        if (sprinting) { sprint(); }
        else
        {
            if (LastHoldedKey && (Time.time-LastClickTime < DoubleTapTime) && (Input.GetKey(Right) || Input.GetKey(Left))) 
            { sprinting = true; }
            Debug.Log(sprinting);
            walk();
        }      
    }



    private void walk()
    {
        if (Input.GetKey(Right) || Input.GetKey(Left)) { LastClickTime = Time.time; LastHoldedKey = false; }
        if (Input.GetKey(Right) && !Input.GetKey(Left))
        {
            PlayerBody.velocity = new Vector2(+WalkForce, PlayerBody.velocity.y);
            PlayerRender.flipX = false;
        }
        else if (Input.GetKey(Left) && !Input.GetKey(Right))
        {
            PlayerBody.velocity = new Vector2(-WalkForce, PlayerBody.velocity.y);
            PlayerRender.flipX = true;
        }
        else{ PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y); LastHoldedKey = true; }
    }

    private void sprint()
    {
        if (Input.GetKey(Right) && !Input.GetKey(Left))
        {
            PlayerBody.velocity = new Vector2(+SprintForce, PlayerBody.velocity.y);
            PlayerRender.flipX = false;
        }
        else if (Input.GetKey(Left) && !Input.GetKey(Right))
        {
            PlayerBody.velocity = new Vector2(-SprintForce, PlayerBody.velocity.y);
            PlayerRender.flipX = true;
        }
        else{ sprinting = false; PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y); }
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

