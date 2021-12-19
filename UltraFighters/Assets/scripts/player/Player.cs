using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    System.Random rrr = new System.Random();
    [Header("LayerMasks")]
    [SerializeField] private LayerMask PlatformLayerMask;
    [SerializeField] private LayerMask PlatformLayerMask2;
    [Header("Health & Saturation")]
    [SerializeField] public int MaxHealth = 200;
    [SerializeField] public int Saturation = 100;
    [Header("Movement")]
    [SerializeField] private float WalkForce = 5f;
    [SerializeField] private float SprintForce = 8f;
    [SerializeField] private float JumpForce = 10f;
    [SerializeField] private float LadderVertical = 5f;
    [SerializeField] private float LadderHorizontal = 3f;
    [Header("Other")]
    [SerializeField] private float DoubleTapTime = 0.2f;
    [SerializeField] private float TimeInRoll = 0.5f;
    [SerializeField] private float ThrowJump = 5f;
    [Header("Gravity settings")]
    [SerializeField] private float ThrowJumpGravity = 2f;
    [SerializeField] private float NormalGravity = 2.5f;
    [SerializeField] private float FallGravity = 3.5f;

    public static string PlayerRotation = "Right";
    private string PlayerLastRotation;
    private float Health;

    //crouching,ladder,onewayplatform variables
    private bool isCrouching = false;
    private bool isOnLadder;
    public static bool isInPlatform = false;

    // walking variables
    private float LastKeyRight = -5f;
    private float LastKeyLeft = -5f;

    //sprinting variables
    private bool sprinting = false;
    private float LastSprintRight = -5f;
    private float LastSprintLeft = -5f;

    [Header("Shooting")]
    [SerializeField] public Transform FirePoint;
    Gun PlayerGun;
    public static bool shooting = false;
    private double LastTimeShoot = -5f;
    private bool ReadyToFire = false;


    [Header("Components")]
    [SerializeField] private Rigidbody2D PlayerBody;
    [SerializeField] private BoxCollider2D PlayerHitBox;
    [SerializeField] private Animator PlayerAnimator;
    
    void Start()
    {
        Health = MaxHealth;
        PlayerGun = GetGun("Pistol");
        PlayerLastRotation = PlayerRotation;
        if (PlayerRotation == "Left") { transform.Rotate(0f, 180f, 0F); }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) { PlayerGun = GetGun("Pistol"); }
        if (Input.GetKeyDown(KeyCode.W)) { PlayerGun = GetGun("Shotgun"); }
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
        AnimationSetter();
    }

    private void move()
    {
        if ((Input.GetKey(GlobalVariables.P1Down) && isGrounded()) || isCrouching)
            crouch();
        else
        {
            jump();
            if (sprinting) { sprint(); }
            else { walk(); }
            if (PlayerBody.velocity.y < 0)
                PlayerBody.gravityScale = FallGravity;
            else
                PlayerBody.gravityScale = NormalGravity;
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
            if (Input.GetKeyDown(GlobalVariables.P1Right) && Time.time - LastKeyRight < DoubleTapTime) { sprinting = true; }
            LastKeyRight = Time.time; PlayerRotation = "Right";
            PlayerBody.velocity = new Vector2(+WalkForce, PlayerBody.velocity.y);
        }
        else if (Input.GetKey(GlobalVariables.P1Left) && !Input.GetKey(GlobalVariables.P1Right))     // walk left
        {   //sprint check
            if (Input.GetKeyDown(GlobalVariables.P1Left) && Time.time - LastKeyLeft < DoubleTapTime) { sprinting = true; }
            LastKeyLeft = Time.time; PlayerRotation = "Left";
            PlayerBody.velocity = new Vector2(-WalkForce, PlayerBody.velocity.y);
        }
        else { PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y); }   //stay at position
    }
    private void sprint()
    {
        if (Input.GetKey(GlobalVariables.P1Right) && !Input.GetKey(GlobalVariables.P1Left)) //sprint right
        {   //check double tap right to walk
            if (Input.GetKeyDown(GlobalVariables.P1Right) && Time.time - LastSprintRight < DoubleTapTime) { sprinting = false; }
            LastSprintRight = Time.time; PlayerRotation = "Right";
            PlayerBody.velocity = new Vector2(+SprintForce, PlayerBody.velocity.y);
        }
        else if (Input.GetKey(GlobalVariables.P1Left) && !Input.GetKey(GlobalVariables.P1Right))  //sprint left
        {   //check double tap left to walk
            if (Input.GetKeyDown(GlobalVariables.P1Left) && Time.time - LastSprintLeft < DoubleTapTime) { sprinting = false; }
            LastSprintLeft = Time.time; PlayerRotation = "Left";
            PlayerBody.velocity = new Vector2(-SprintForce, PlayerBody.velocity.y);
        }
        else
        {   //if you  stay more than "DoubleTapTime" it will remove sprint
            if (Time.time - LastSprintLeft > DoubleTapTime && Time.time - LastSprintRight > DoubleTapTime) { sprinting = false; }
            PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y);
        }
    }
    private void jump()
    {
        if (Input.GetKey(GlobalVariables.P1Up) && isGrounded() && (!Input.GetKey(GlobalVariables.P1Down)) && (!isCrouching))
            PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, JumpForce);
    }

    private void Ladder()
    {
        PlayerBody.gravityScale = 0f;
        // pohyb vpravo a vlavo
        if (Input.GetKey(GlobalVariables.P1Right) && !Input.GetKey(GlobalVariables.P1Left))
        { PlayerBody.velocity = new Vector2(+LadderHorizontal, PlayerBody.velocity.y); PlayerRotation = "Right"; }
        else if (Input.GetKey(GlobalVariables.P1Left) && !Input.GetKey(GlobalVariables.P1Right))
        { PlayerBody.velocity = new Vector2(-LadderHorizontal, PlayerBody.velocity.y); PlayerRotation = "Left"; }
        else { PlayerBody.velocity = new Vector2(0f, PlayerBody.velocity.y); }
        // pohyb hore a dole
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
                PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, ThrowJump);
            }
            else if (isGrounded())
            {
                if (PlayerBody.velocity.x > 0) { PlayerBody.velocity = new Vector2(WalkForce, 0f); }
                else if (PlayerBody.velocity.x < 0) { PlayerBody.velocity = new Vector2(-WalkForce, 0f); }
                StartCoroutine(Roll());
                PlayerBody.gravityScale = NormalGravity;
            }
        }
    }
    private IEnumerator Roll()
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
        
        else if (ReadyToFire && PlayerGun.name != "" && PlayerGun.name != "None")
        {   
            ReadyToFire = false;
            if (PlayerGun.fire())
            {
                if (PlayerGun.name is "Shotgun")
                {
                    Quaternion idk1 = Quaternion.Euler(0, 0, 5);
                    Vector3 idk2 = new Vector3(0, 0, 1);
                    Instantiate(PlayerGun.P1_Bullet, FirePoint.position - idk2, QuaternionDifference(idk1, FirePoint.rotation));
                    Instantiate(PlayerGun.P1_Bullet, FirePoint.position, FirePoint.rotation);
                }
                else
                {
                    Instantiate(PlayerGun.P1_Bullet, FirePoint.position, FirePoint.rotation);
                }
            }          
        }
        
    }

    private bool isGrounded()
    {
        RaycastHit2D rayCastHit1 = Physics2D.Raycast(PlayerHitBox.bounds.center, Vector2.down, PlayerHitBox.bounds.extents.y - 0.2f, PlatformLayerMask2);
        RaycastHit2D rayCastHit2 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x + PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y), Vector2.down, PlayerHitBox.bounds.extents.y - 0.2f, PlatformLayerMask2);
        RaycastHit2D rayCastHit3 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x - PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y), Vector2.down, PlayerHitBox.bounds.extents.y - 0.2f, PlatformLayerMask2);
        if ((rayCastHit1.collider != null || rayCastHit2.collider != null || rayCastHit3.collider != null) && (!isInPlatform))
        {
            StartCoroutine(IsInPlatform());
            return false;
        }
        else if (!isInPlatform)
        {
            rayCastHit1 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, PlatformLayerMask);
            rayCastHit2 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x + PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, PlatformLayerMask);
            rayCastHit3 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x - PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, PlatformLayerMask);
            if (rayCastHit1.collider != null || rayCastHit2.collider != null || rayCastHit3.collider != null)
                return true; 
            else
                return false; 
        }
        else
            return false;
    }
    private IEnumerator IsInPlatform()
    {
        isInPlatform = true;
        yield return new WaitForSeconds(0.5f);
        isInPlatform = false;
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
            if (!isCrouching)
                PlayerBody.gravityScale = 2.5f;
            isOnLadder = false;
        }
    }

    private void HitBoxChanger(float sizeX, float sizeY, float offsetX, float offsetY)
    {
        PlayerHitBox.size = new Vector2(sizeX, sizeY);
        PlayerHitBox.offset = new Vector2(offsetX, offsetY);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
    private void AnimationSetter()
    {
        PlayerAnimator.SetBool("isGrounded", isGrounded());
        PlayerAnimator.SetBool("isCrouching", isCrouching);
        PlayerAnimator.SetBool("isOnLadder", isOnLadder);
        PlayerAnimator.SetFloat("PlayerSpeed", Math.Abs(PlayerBody.velocity.x));
    }
    private Gun GetGun(string name)
    {
        GameObject GM = GameObject.Find("LevelManager");
        GunManager GunM = GM.GetComponent<GunManager>();
        foreach (var Gunitem in GunM.AllGuns){
            if (name == Gunitem.name) { return Gunitem.Clone(); }
        }
        return GunM.AllGuns[0];
    }
    private Quaternion QuaternionDifference(Quaternion origin, Quaternion target)
    {
        Quaternion identityOrigin = Quaternion.identity * Quaternion.Inverse(origin);
        Quaternion identityTarget = Quaternion.identity * Quaternion.Inverse(target);
        return identityOrigin * Quaternion.Inverse(identityTarget);
    }
}
