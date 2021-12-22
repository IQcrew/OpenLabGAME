using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    System.Random rrr = new System.Random();
    [Header("LayerMasks")]
    [SerializeField] private LayerMask PlatformLayerMask;
    [SerializeField] private LayerMask OneWayPlatformLayerMask;
    [Header("Health & Saturation")]
    [SerializeField] public string PlayerName;
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
    private int BulletsToShot = 0;
    private Quaternion TempQuaternion = Quaternion.Euler(0, 0, 0);
    private Laser MyLaser;

    [Header("Components")]
    [SerializeField] private Rigidbody2D PlayerBody;
    [SerializeField] private BoxCollider2D PlayerHitBox;
    [SerializeField] private Animator PlayerAnimator;
    
    void Start()
    {
        GameObject LaserClass = GameObject.Find("LaserPoint");
        MyLaser = LaserClass.GetComponent<Laser>();
        Health = MaxHealth;
        PlayerGun = GetGun("Pistol");
        PlayerLastRotation = PlayerRotation;
        if (PlayerRotation == "Left") { transform.Rotate(0f, 180f, 0F); }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) { PlayerGun = GetGun("AssalutRifle"); }
        if (Input.GetKeyDown(KeyCode.W)) { PlayerGun = GetGun("SniperRifle"); }
        if (LastTimeShoot + 0.5 < Time.time || !Input.GetKey(GlobalVariables.P1fire) && (Input.GetKey(GlobalVariables.P1Right) || Input.GetKey(GlobalVariables.P1Left) || Input.GetKey(GlobalVariables.P1Up) || Input.GetKey(GlobalVariables.P1Down) || Input.GetKey(GlobalVariables.P1hit) || Input.GetKey(GlobalVariables.P1slot))) { 
            shooting = false; MyLaser.ShootLaser(false); }
        if (isOnLadder && (!isCrouching) && (!isGrounded()))
            Ladder();
        else if (PlayerGun.name != "None" && isGrounded() && (Input.GetKey(GlobalVariables.P1fire) || shooting))
        {
            shooting = true;
            ShootPosition();
            PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y);
        }
        else{
            BulletsToShot = 0;
            move();
        }
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
            if (!isCrouching) { HitBoxChanger(1.2f, 1.7f, 0f, -0.323f, true); }
            else if (!Input.GetKey(GlobalVariables.P1Down)) { HitBoxChanger(1.2f, 2.2f, 0f, -0.075f, false); }
        }
        else if (new Vector2(Math.Abs(PlayerBody.velocity.x), 0f) == new Vector2(WalkForce, 0f))
        {
            if (!isCrouching) { StartCoroutine(Roll()); }
        }
        else if (new Vector2(Math.Abs(PlayerBody.velocity.x), 0f) == new Vector2(SprintForce, 0f))
        {
            if (!isCrouching)
            {
                HitBoxChanger(2f, 1f, 0f, 0f, true);
                PlayerBody.gravityScale = ThrowJumpGravity;
                PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, ThrowJump);
            }
            else if (isGrounded())
            {
                if (PlayerBody.velocity.x > 0) { PlayerBody.velocity = new Vector2(WalkForce, 0f); }
                else if (PlayerBody.velocity.x < 0) { PlayerBody.velocity = new Vector2(-WalkForce, 0f); }
                PlayerBody.gravityScale = NormalGravity;
                StartCoroutine(Roll());
            }
        }
    }
    private IEnumerator Roll()
    {
        HitBoxChanger(1.2f, 1.2f, 0f, -0.575f, true);
        yield return new WaitForSeconds(TimeInRoll);
        HitBoxChanger(1.2f, 2.2f, 0f, -0.075f, false);
    }
    private void HitBoxChanger(float sizeX, float sizeY, float offsetX, float offsetY, bool isCrouching)
    {
        PlayerHitBox.size = new Vector2(sizeX, sizeY);
        PlayerHitBox.offset = new Vector2(offsetX, offsetY);
        this.isCrouching = isCrouching;
    }

    private void ShootPosition()
    {
        if (BulletsToShot > 0){
            switch (PlayerGun.name)
            {
                case "Mac-10":
                    if (Time.time > LastTimeShoot + 0.15f){
                        TempQuaternion = Quaternion.Euler(0, 0, (((float)rrr.NextDouble()) * 10) - 5);
                        Instantiate(PlayerGun.Bullet, FirePoint.position, QuaternionDifference(TempQuaternion, FirePoint.rotation));
                        BulletsToShot -= 1; PlayerGun.ammo -= 1; LastTimeShoot = Time.time;}
                    break;
                case "AssalutRifle":
                    if (Time.time > LastTimeShoot + 0.1f){
                        Instantiate(PlayerGun.Bullet, FirePoint.position, FirePoint.rotation);
                        BulletsToShot -= 1; PlayerGun.ammo -= 1; LastTimeShoot = Time.time;}
                    break;
            }
            return;
        }
        if(PlayerGun.name is "SniperRifle") { MyLaser.ShootLaser(true); }
        else { MyLaser.ShootLaser(false); }
        if (Input.GetKeyDown(GlobalVariables.P1hit) || Input.GetKeyDown(GlobalVariables.P1slot)) { shooting = false; MyLaser.ShootLaser(false); }
        if (Input.GetKey(GlobalVariables.P1fire)) { ReadyToFire = true; }    
        else if (ReadyToFire && PlayerGun.name != "" && PlayerGun.name != "None")
        {   
            ReadyToFire = false;
            if (PlayerGun.fire()){
                switch (PlayerGun.name){
                    case "Shotgun":
                        for (int i = 0; i < 4; i++){
                            TempQuaternion = Quaternion.Euler(0, 0, (((float)rrr.NextDouble()) * 14) - 7);
                            Instantiate(PlayerGun.Bullet, FirePoint.position, QuaternionDifference(TempQuaternion, FirePoint.rotation)); 
                        }
                        PlayerGun.ammo -= 1;
                        break;
                    case "Mac-10":
                    case "AssalutRifle":
                        BulletsToShot = PlayerGun.BulletsOnShoot;
                        break;
                    default:
                        Instantiate(PlayerGun.Bullet, FirePoint.position, FirePoint.rotation);
                        PlayerGun.ammo -= 1;
                        break;
                }  
            }          
        }
        if (Input.GetKey(GlobalVariables.P1fire)) { LastTimeShoot = Time.time; }
        if (PlayerGun.ammo <= 0){ PlayerGun = GetGun("None"); }
    }

    private bool isGrounded()
    {
        RaycastHit2D rayCastHit1 = Physics2D.Raycast(PlayerHitBox.bounds.center, Vector2.down, PlayerHitBox.bounds.extents.y - 0.2f, OneWayPlatformLayerMask);
        RaycastHit2D rayCastHit2 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x + PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y), Vector2.down, PlayerHitBox.bounds.extents.y - 0.2f, OneWayPlatformLayerMask);
        RaycastHit2D rayCastHit3 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x - PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y), Vector2.down, PlayerHitBox.bounds.extents.y - 0.2f, OneWayPlatformLayerMask);
        if (isInPlatform)
            return false;
        else if (rayCastHit1.collider != null || rayCastHit2.collider != null || rayCastHit3.collider != null)
        {
            StartCoroutine(IsInPlatform());
            return false;
        }
        else
        {
            rayCastHit1 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, PlatformLayerMask);
            rayCastHit2 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x + PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, PlatformLayerMask);
            rayCastHit3 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x - PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, PlatformLayerMask);
            if ((rayCastHit1.collider != null || rayCastHit2.collider != null || rayCastHit3.collider != null))
                return true; 
            else
                return false; 
        }
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
        if (PlayerBody.velocity != Vector2.zero)
            PlayerAnimator.SetBool("IsLadderMoving", true);
        else
            PlayerAnimator.SetBool("IsLadderMoving", false);
    }
    private Gun GetGun(string name)
    {
        GameObject GM = GameObject.Find("LevelManager");
        GunManager GunM = GM.GetComponent<GunManager>();
        foreach (var Gunitem in GunM.AllGuns){
            if (name == Gunitem.name) { 
                Gun TempGun = Gunitem.Clone();
                bullet TempBullet = TempGun.Bullet.GetComponent<bullet>();
                TempBullet.shooter_name = PlayerName;
                TempBullet.damage = TempGun.damage;
                return TempGun;
            }
        }
        return GunM.AllGuns[0];
    }
    private Quaternion QuaternionDifference(Quaternion origin, Quaternion target)
    {
        Quaternion identityOrigin = Quaternion.identity * Quaternion.Inverse(origin);
        Quaternion identityTarget = Quaternion.identity * Quaternion.Inverse(target);
        return identityOrigin * Quaternion.Inverse(identityTarget);
    }
    public void PickUpItem(string Name)
    {
        if(Name is "Heal") { Health = MaxHealth; }
        else if (PlayerGun.name is "None") { GetGun(Name); }
        else{
            if (Input.GetKey(GlobalVariables.P1hit) && Input.GetKey(GlobalVariables.P2Down)){ GetGun(Name); }
        }
    }
}
