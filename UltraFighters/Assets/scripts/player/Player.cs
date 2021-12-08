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
    public string PlayerRotation = "Right";
    private string PlayerLastRotation;

    private bool isOnLadder;

    // crouching variables
    private float timer;
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



    Rigidbody2D PlayerBody;
    BoxCollider2D PlayerHitBox;
    SpriteRenderer PlayerRender;
    Animator PlayerAnimation;

    // Start is called before the first frame update
    void Start()
    {
        PlayerBody = GetComponent<Rigidbody2D>();
        PlayerHitBox = GetComponent<BoxCollider2D>();
        PlayerRender = GetComponent<SpriteRenderer>();
        PlayerAnimation = GetComponent<Animator>();
        PlayerLastRotation = PlayerRotation;
        if (PlayerRotation == "Left") { transform.Rotate(0f, 180f, 0F); }
    }

    // Update is called once per frame
    void Update()
    {
        if (LastTimeShoot + 0.5 < Time.time || !Input.GetKey(GlobalVariables.P1fire) && (Input.GetKey(GlobalVariables.P1Right) || Input.GetKey(GlobalVariables.P1Left) || Input.GetKey(GlobalVariables.P1Up) || Input.GetKey(GlobalVariables.P1Down) || Input.GetKey(GlobalVariables.P1hit) || Input.GetKey(GlobalVariables.P1slot)))
            shooting = false;
        if (isOnLadder)
            Ladder();
        else if (isGrounded() && (Input.GetKey(GlobalVariables.P1fire) || shooting))
        {
            if (Input.GetKey(GlobalVariables.P1fire)) { LastTimeShoot = Time.time; }
            shooting = true;
            ShootPosition();
            PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y);
        }
        else
        {
            move();
            jump();
        }
    }
    private void move()
    {
        PlayerAnimation.SetFloat("PlayerVelocity", Math.Abs(PlayerBody.velocity.x));
        if ((Input.GetKey(GlobalVariables.P1Down) && isGrounded()) || isCrouching)
            crouch();
        else
        {
            if (sprinting) 
            { sprint(); }
            else 
            { walk(); }
        }
        //Fliping Player
        if (PlayerRotation == "Right" && PlayerLastRotation != "Right") { transform.Rotate(0f, 180f, 0F); }
        else if (PlayerRotation == "Left" && PlayerLastRotation != "Left") { transform.Rotate(0f, 180f, 0F); }
        PlayerLastRotation = PlayerRotation;
    }

    private void Ladder()
    {
        PlayerAnimation.SetFloat("PlayerVelocity", 0f);
        if (Input.GetKey(GlobalVariables.P1Right) && !Input.GetKey(GlobalVariables.P1Left))
            { PlayerBody.velocity = new Vector2(+3f, PlayerBody.velocity.y); }
        else if (Input.GetKey(GlobalVariables.P1Left) && !Input.GetKey(GlobalVariables.P1Right)) 
            { PlayerBody.velocity = new Vector2(-3f, PlayerBody.velocity.y);}
        else { PlayerBody.velocity = new Vector2(0f, PlayerBody.velocity.y); }

        if (Input.GetKey(GlobalVariables.P1Up) && (!Input.GetKey(GlobalVariables.P1Down)))
        { PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, +5f); }
        else if (Input.GetKey(GlobalVariables.P1Down) && (!Input.GetKey(GlobalVariables.P1Up)))
        { PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, -5f); }
        else { PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, 0f); }
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
            if (Input.GetKeyDown(GlobalVariables.P1Right) && Time.time - LastSprintRight < DoubleTapTime){ sprinting = false;}
            LastSprintRight = Time.time; PlayerRotation = "Right";
            PlayerBody.velocity = new Vector2(+SprintForce, PlayerBody.velocity.y);
        }
        else if (Input.GetKey(GlobalVariables.P1Left) && !Input.GetKey(GlobalVariables.P1Right))  //sprint left
        {   //check double tap left to walk
            if (Input.GetKeyDown(GlobalVariables.P1Left) && Time.time - LastSprintLeft < DoubleTapTime){sprinting = false;}
            LastSprintLeft = Time.time; PlayerRotation = "Left";
            PlayerBody.velocity = new Vector2(-SprintForce, PlayerBody.velocity.y);
        }
        else{   //if you  stay more than "DoubleTapTime" it will remove sprint
            if (Time.time-LastSprintLeft > DoubleTapTime && Time.time - LastSprintRight > DoubleTapTime) {sprinting = false; }
            PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y); }
    }

    private void jump()
    {
        if (Input.GetKey(GlobalVariables.P1Up) && isGrounded() && (!Input.GetKey(GlobalVariables.P1Down)) && (!isCrouching))
        {
            PlayerBody.velocity = Vector2.up * JumpForce;
        }
    }

    private void ShootPosition()
    {
        if (Input.GetKeyDown(GlobalVariables.P1hit) || Input.GetKeyDown(GlobalVariables.P1slot)) { shooting = false; }
        
    }

    private void crouch()
    {
        if (PlayerBody.velocity == Vector2.zero)
        {
            if (Input.GetKey(GlobalVariables.P1Down))
            {
                isCrouching = true;
                PlayerHitBox.size = new Vector2(PlayerHitBox.size.x, 1.69f);
                PlayerHitBox.offset = new Vector2(PlayerHitBox.offset.x, -0.4f);
            }
            else
            {
                isCrouching = false;
                PlayerHitBox.size = new Vector2(PlayerHitBox.size.x, 2.3f);
                PlayerHitBox.offset = new Vector2(PlayerHitBox.offset.x, -0.095f);
            }   
        }
        else if ((PlayerBody.velocity == new Vector2(WalkForce, 0f)) || (PlayerBody.velocity == new Vector2(-WalkForce, 0f)))
        {
            if (((Input.GetKey(GlobalVariables.P1Right) && (!Input.GetKey(GlobalVariables.P1Left))) || (Input.GetKey(GlobalVariables.P1Left) && (!Input.GetKey(GlobalVariables.P1Right)))) && (!isCrouching))
            {
                isCrouching = true;
                StartCoroutine(Roll());
            }
            else if (!isCrouching)
                PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            PlayerBody.gravityScale = 0f;
            isOnLadder = true;
        }
            
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            PlayerBody.gravityScale = 2.5f;
            isOnLadder = false;
        }
            
    }

    IEnumerator Roll()
    {
        PlayerHitBox.size = new Vector2(PlayerHitBox.size.x, 1.2f);
        PlayerHitBox.offset = new Vector2(PlayerHitBox.offset.x, -0.64f);
        yield return new WaitForSeconds(0.75f);
        PlayerHitBox.size = new Vector2(PlayerHitBox.size.x, 2.3f);
        PlayerHitBox.offset = new Vector2(PlayerHitBox.offset.x, -0.095f);
        isCrouching = false;
    }
}

