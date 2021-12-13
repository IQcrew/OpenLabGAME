using System.Collections;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask PlatformLayerMask;
    [SerializeField] private float WalkForce = 5f;
    [SerializeField] private float SprintForce = 8f;
    [SerializeField] private float JumpForce = 10f;
    [SerializeField] private float DoubleTapTime = 0.2f;
    [SerializeField] private float LadderVertical = 5f;
    [SerializeField] private float LadderHorizontal = 3f;
    [SerializeField] private float TimeInRoll = 0.5f;
    [SerializeField] private float ThrowJump = 5f;
    [SerializeField] private float ThrowJumpGravity = 2f;

    public static string PlayerRotation = "Right";
    private string PlayerLastRotation;

    private bool isOnLadder;
    private bool isCrouching = false;

    // walking variables
    private float LastKeyRight = -5f;
    private float LastKeyLeft = -5f;

    //sprinting variables
    private bool sprinting = false;
    private float LastSprintRight = -5f;
    private float LastSprintLeft = -5f;

    //shooting
    public static bool shooting = false;
    private double LastTimeShoot = -5f;
    public Transform FirePoint;
    private bool ReadyToFire = false;
    public Gun[] AllGuns = new Gun[2];

    //player components
    Rigidbody2D PlayerBody;
    BoxCollider2D PlayerHitBox;
    Animator PlayerAnimation;

    void Start()
    {
        PlayerBody = GetComponent<Rigidbody2D>();
        PlayerHitBox = GetComponent<BoxCollider2D>();
        PlayerAnimation = GetComponent<Animator>();
        PlayerLastRotation = PlayerRotation;
        if (PlayerRotation == "Left") { transform.Rotate(0f, 180f, 0F); }
    }

    void Update()
    {
        if (LastTimeShoot + 0.5 < Time.time || !Input.GetKey(GlobalVariables.P1fire) && (Input.GetKey(GlobalVariables.P1Right) || Input.GetKey(GlobalVariables.P1Left) || Input.GetKey(GlobalVariables.P1Up) || Input.GetKey(GlobalVariables.P1Down) || Input.GetKey(GlobalVariables.P1hit) || Input.GetKey(GlobalVariables.P1slot)))
            shooting = false;
        if (isOnLadder && (!isCrouching) && (!isGrounded())) 
            Ladder(); 
        else if (isGrounded() && (Input.GetKey(GlobalVariables.P1fire) || shooting))
        {
            if (Input.GetKey(GlobalVariables.P1fire)) { LastTimeShoot = Time.time; }
            shooting = true;
            ShootPosition();
            PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y);
        }
        else 
            move();
    }

    private void move()
    {
        PlayerAnimation.SetFloat("PlayerVelocity", Math.Abs(PlayerBody.velocity.x));
        if ((Input.GetKey(GlobalVariables.P1Down) && isGrounded()) || isCrouching)
            crouch();
        else
        {
            if (sprinting) { sprint(); }
            else { walk(); }
            jump();
        }
        //Fliping Player
        if (PlayerRotation == "Right" && PlayerLastRotation != "Right") { transform.Rotate(0f, 180f, 0F); }
        else if (PlayerRotation == "Left" && PlayerLastRotation != "Left") { transform.Rotate(0f, 180f, 0F); }
        PlayerLastRotation = PlayerRotation;
    }

    private void walk()
    {
        if (Input.GetKey(GlobalVariables.P1Right) && !Input.GetKey(GlobalVariables.P1Left)) //walk right
        {   //sprint check
            if (Input.GetKeyDown(GlobalVariables.P1Right) && Time.time - LastKeyRight < DoubleTapTime) {sprinting = true; }   
            LastKeyRight = Time.time; PlayerRotation = "Right";
            PlayerBody.velocity = new Vector2(+WalkForce, PlayerBody.velocity.y);
        }
        else if (Input.GetKey(GlobalVariables.P1Left) && !Input.GetKey(GlobalVariables.P1Right))     // walk left
        {   //sprint check
            if (Input.GetKeyDown(GlobalVariables.P1Left) && Time.time - LastKeyLeft < DoubleTapTime){sprinting = true;}          
            LastKeyLeft = Time.time; PlayerRotation = "Left";   
            PlayerBody.velocity = new Vector2(-WalkForce, PlayerBody.velocity.y);
        }
        else { PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y); }   //stay at position
    }
    private void sprint()
    {
        if (Input.GetKey(GlobalVariables.P1Right) && !Input.GetKey(GlobalVariables.P1Left)) //sprint right
        {   //check double tap right to walk
            if (Input.GetKeyDown(GlobalVariables.P1Right) && Time.time - LastSprintRight < DoubleTapTime) { sprinting = false;}
            LastSprintRight = Time.time; PlayerRotation = "Right";
            PlayerBody.velocity = new Vector2(+SprintForce, PlayerBody.velocity.y);
        }
        else if (Input.GetKey(GlobalVariables.P1Left) && !Input.GetKey(GlobalVariables.P1Right))  //sprint left
        {   //check double tap left to walk
            if (Input.GetKeyDown(GlobalVariables.P1Left) && Time.time - LastSprintLeft < DoubleTapTime) {sprinting = false;}
            LastSprintLeft = Time.time; PlayerRotation = "Left";
            PlayerBody.velocity = new Vector2(-SprintForce, PlayerBody.velocity.y);
        }
        else {   //if you  stay more than "DoubleTapTime" it will remove sprint
            if (Time.time-LastSprintLeft > DoubleTapTime && Time.time - LastSprintRight > DoubleTapTime) {sprinting = false; }
            PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y); }
    }
    private void jump()
    {
        if (Input.GetKey(GlobalVariables.P1Up) && isGrounded() && (!Input.GetKey(GlobalVariables.P1Down)) && (!isCrouching))
            PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, JumpForce) ;
    }

    private void Ladder()
    {
        PlayerAnimation.SetFloat("PlayerVelocity", 0f);
        if (Input.GetKey(GlobalVariables.P1Right) && !Input.GetKey(GlobalVariables.P1Left))
            { PlayerBody.velocity = new Vector2(+LadderHorizontal, PlayerBody.velocity.y); PlayerRotation = "Right"; }
        else if (Input.GetKey(GlobalVariables.P1Left) && !Input.GetKey(GlobalVariables.P1Right)) 
            { PlayerBody.velocity = new Vector2(-LadderHorizontal, PlayerBody.velocity.y); PlayerRotation = "Left"; }
        else { PlayerBody.velocity = new Vector2(0f, PlayerBody.velocity.y); }

        if (Input.GetKey(GlobalVariables.P1Up) && (!Input.GetKey(GlobalVariables.P1Down)))
        { PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, +LadderVertical); }
        else if (Input.GetKey(GlobalVariables.P1Down) && (!Input.GetKey(GlobalVariables.P1Up)))
        { PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, -LadderVertical); }
        else { PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, 0f); }
    }
    private void crouch()
    {
        if (PlayerBody.velocity == Vector2.zero)
        {
            if (Input.GetKey(GlobalVariables.P1Down))
            {
                isCrouching = true;
                HitBoxChanger(1.2f, 1.7f, 0f, -0.323f);
            }
            else
            {
                isCrouching = false;
                HitBoxChanger(1.2f, 2.2f, 0f, -0.075f);
            }   
        }
        else if ((PlayerBody.velocity == new Vector2(WalkForce, 0f)) || (PlayerBody.velocity == new Vector2(-WalkForce, 0f)))
        {
            if (((Input.GetKey(GlobalVariables.P1Right) && (!Input.GetKey(GlobalVariables.P1Left))) || (Input.GetKey(GlobalVariables.P1Left) && (!Input.GetKey(GlobalVariables.P1Right)))) && (!isCrouching))
            {
                isCrouching = true;
                StartCoroutine(Roll());
            }
            else if (!isCrouching) { PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y); }
        }
        else if (PlayerBody.velocity == new Vector2(SprintForce, 0f) || PlayerBody.velocity == new Vector2(-SprintForce, 0f))
        {
            if (!isCrouching)
            {
                isCrouching = true;
                HitBoxChanger(2f, 1f, 0f, 0f);
                PlayerBody.gravityScale = ThrowJumpGravity;
                PlayerBody.velocity = new Vector2(PlayerBody.velocity.x,ThrowJump);
            }
            else if(isGrounded())
            {
                if (PlayerBody.velocity.x > 0) { PlayerBody.velocity = new Vector2(WalkForce, 0f); }
                else if (PlayerBody.velocity.x < 0) { PlayerBody.velocity = new Vector2(-WalkForce, 0f); }
                StartCoroutine(Roll());
                PlayerBody.gravityScale = 2.5f;
            }
        }  
    }
    IEnumerator Roll()
    {
        HitBoxChanger(1.2f, 1.2f, 0f, -0.575f);
        yield return new WaitForSeconds(TimeInRoll);
        HitBoxChanger(1.2f, 2.2f, 0f, -0.075f);
        isCrouching = false;
    }

    private void ShootPosition()
    {
        if (Input.GetKeyDown(GlobalVariables.P1hit) || Input.GetKeyDown(GlobalVariables.P1slot)) { shooting = false; }
        if (Input.GetKey(GlobalVariables.P1fire)) { ReadyToFire = true; }
        else if (ReadyToFire)
        {
            ReadyToFire = false;
            if (AllGuns[1].fire()) { Instantiate(AllGuns[1].Bullet, FirePoint.position, FirePoint.rotation); }
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(PlayerHitBox.bounds.center, new Vector2(PlayerHitBox.bounds.size.x*0.9f, PlayerHitBox.bounds.size.y), 0f, Vector2.down, 0.025f, PlatformLayerMask);
            return rayCastHit.collider != null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            if (!isCrouching)
                PlayerBody.gravityScale = 0f;
            isOnLadder = true;
        }
            
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            if(!isCrouching)
                PlayerBody.gravityScale = 2.5f;
            isOnLadder = false;
        }
    }

    private void HitBoxChanger(float sizeX, float sizeY, float offsetX, float offsetY)
    {
        PlayerHitBox.size = new Vector2(sizeX, sizeY);
        PlayerHitBox.offset = new Vector2(offsetX, offsetY);
    }
}