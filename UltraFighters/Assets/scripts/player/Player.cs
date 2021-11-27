using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask PlatformLayerMask;
    [SerializeField] private float WalkForce = 5f;
    [SerializeField] private float SprintForce = 8f;
    [SerializeField] private float JumpForce = 5f;
    [SerializeField] private float DoubleTapTime = 0.2f;

    // crouching variables
    private float timer;
    private bool isCrouching = false;

    // walking variables
    private float LastKeyRight = -5f;
    private float LastKeyLeft = -5f;
    private bool LastFreeKeys = false;

    //sprinting variables
    private bool sprinting = false;
    private bool LastFreeKeysSprint = false;
    private float LastSprintRight = -5f;
    private float LastSprintLeft = -5f;



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
        move();
        jump();
    }
    private void move()
    {
        if (Input.GetKey(Down) || isCrouching)
            crouch();
        else
        {
            PlayerHitBox.size = new Vector2(PlayerHitBox.size.x, 2.3f);
            PlayerHitBox.offset = new Vector2(PlayerHitBox.offset.x, -0.095f);
            if (sprinting) 
            { sprint(); }
            else 
            { walk(); }
        }
       
    }

    private void walk()
    {
        if (Input.GetKey(Right) && !Input.GetKey(Left))
        {
            PlayerRender.flipX = false;
            if (LastFreeKeys && Time.time-LastKeyRight < DoubleTapTime) {
                sprinting = true;
                sprint();
                return;
            }
            LastKeyRight = Time.time;  LastFreeKeys = false;
            PlayerBody.velocity = new Vector2(+WalkForce, PlayerBody.velocity.y);
        }
        else if (Input.GetKey(Left) && !Input.GetKey(Right))
        {
            PlayerRender.flipX = true;
            if (LastFreeKeys && Time.time - LastKeyLeft < DoubleTapTime) { 
                sprinting = true;
                sprint();
                return;}
            LastKeyLeft = Time.time; LastFreeKeys = false;
            PlayerBody.velocity = new Vector2(-WalkForce, PlayerBody.velocity.y);
        }
        else { PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y); LastFreeKeys = true; }
    }

    private void sprint()
    {
        if (Input.GetKey(Right) && !Input.GetKey(Left))
        {
            if (LastFreeKeysSprint && Time.time - LastSprintRight < DoubleTapTime){
                sprinting = false;
                walk();
                return;
            }
            PlayerBody.velocity = new Vector2(+SprintForce, PlayerBody.velocity.y);
            PlayerRender.flipX = false;
            LastSprintRight = Time.time;
            LastFreeKeysSprint = false;
        }
        else if (Input.GetKey(Left) && !Input.GetKey(Right))
        {
            if (LastFreeKeysSprint && Time.time-LastSprintLeft < DoubleTapTime){
                sprinting = false;
                walk();
                return;
            }
            PlayerBody.velocity = new Vector2(-SprintForce, PlayerBody.velocity.y);
            PlayerRender.flipX = true;
            LastSprintLeft = Time.time;
            LastFreeKeysSprint = false;
        }
        else{ 
            if (Time.time-LastSprintLeft > DoubleTapTime && Time.time - LastSprintRight > DoubleTapTime) {sprinting = false; }
            LastFreeKeysSprint = true;
            PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y); }
    }

    private void jump()
    {
        if (Input.GetKey(Up) && isGrounded() && (!Input.GetKey(Down)) && (!isCrouching))
        {
            PlayerBody.velocity = Vector2.up * JumpForce;
        }
    }

    private void crouch()
    {
        if (PlayerBody.velocity == Vector2.zero)
        {
            PlayerHitBox.size = new Vector2(PlayerHitBox.size.x, 1.69f);
            PlayerHitBox.offset = new Vector2(PlayerHitBox.offset.x, -0.4f);
        }
        else if (PlayerBody.velocity == new Vector2(WalkForce, 0f)|| PlayerBody.velocity == new Vector2(-WalkForce, 0f))
        {
            isCrouching = true;
            StartCoroutine(TimerCoroutine());
        }
        else if (PlayerBody.velocity == new Vector2(SprintForce, 0f)|| PlayerBody.velocity == new Vector2(-SprintForce, 0f))
        {
            PlayerBody.velocity = new Vector2(0f, PlayerBody.velocity.y);
            // hodenie sa
        }
        
    }

    private bool isGrounded()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(PlayerHitBox.bounds.center, PlayerHitBox.bounds.size, 0f, Vector2.down, 0.025f, PlatformLayerMask);
            return rayCastHit.collider != null;
    }

    IEnumerator TimerCoroutine()
    {
        PlayerHitBox.size = new Vector2(PlayerHitBox.size.x, 1.2f);
        PlayerHitBox.offset = new Vector2(PlayerHitBox.offset.x, -0.64f);
        yield return new WaitForSeconds(0.75f);
        PlayerHitBox.size = new Vector2(PlayerHitBox.size.x, 2.3f);
        PlayerHitBox.offset = new Vector2(PlayerHitBox.offset.x, -0.095f);
        isCrouching = false;
    }
}

