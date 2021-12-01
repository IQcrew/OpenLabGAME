using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask PlatformLayerMask;
    [SerializeField] private float WalkForce = 5f;
    [SerializeField] private float SprintForce = 8f;
    [SerializeField] private float JumpForce = 5f;
    [SerializeField] private float DoubleTapTime = 0.2f;
    public string PlayerRotation = "Right";
    private string PlayerLastRotation;

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
        PlayerLastRotation = PlayerRotation;
        if (PlayerRotation == "Left") { transform.Rotate(0f, 180f, 0F); }
    }

    // Update is called once per frame
    void Update()
    {
        move();
        jump();
    }
    private void move()
    {
        if ((Input.GetKey(Down) && isGrounded()) || isCrouching)
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

    private void walk()
    {
        if (Input.GetKey(Right) && !Input.GetKey(Left)) //walk right
        {   //sprint check
            if (Input.GetKeyDown(Right) && Time.time - LastKeyRight < DoubleTapTime) {sprinting = true; }   
            LastKeyRight = Time.time; PlayerRotation = "Right";
            PlayerBody.velocity = new Vector2(+WalkForce, PlayerBody.velocity.y);
        }
        else if (Input.GetKey(Left) && !Input.GetKey(Right))     // walk left
        {   //sprint check
            if (Input.GetKeyDown(Left) && Time.time - LastKeyLeft < DoubleTapTime){sprinting = true;}          
            LastKeyLeft = Time.time; PlayerRotation = "Left";   
            PlayerBody.velocity = new Vector2(-WalkForce, PlayerBody.velocity.y);
        }
        else { PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y); }   //stay at position
    }

    private void sprint()
    {
        if (Input.GetKey(Right) && !Input.GetKey(Left)) //sprint right
        {   //check double tap right to walk
            if (Input.GetKeyDown(Right) && Time.time - LastSprintRight < DoubleTapTime){ sprinting = false;}
            LastSprintRight = Time.time; PlayerRotation = "Right";
            PlayerBody.velocity = new Vector2(+SprintForce, PlayerBody.velocity.y);
        }
        else if (Input.GetKey(Left) && !Input.GetKey(Right))  //sprint left
        {   //check double tap left to walk
            if (Input.GetKeyDown(Left) && Time.time - LastSprintLeft < DoubleTapTime){sprinting = false;}
            LastSprintLeft = Time.time; PlayerRotation = "Left";
            PlayerBody.velocity = new Vector2(-SprintForce, PlayerBody.velocity.y);
        }
        else{   //if you  stay more than "DoubleTapTime" it will remove sprint
            if (Time.time-LastSprintLeft > DoubleTapTime && Time.time - LastSprintRight > DoubleTapTime) {sprinting = false; }
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
            if (Input.GetKey(Down))
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
            if (((Input.GetKey(Right) && (!Input.GetKey(Left))) || (Input.GetKey(Left) && (!Input.GetKey(Right)))) && (!isCrouching))
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

