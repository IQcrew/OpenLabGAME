using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    System.Random rrr = new System.Random();
    [Header("KeyBinds")]
    [SerializeField] public KeyCode Right = KeyCode.RightArrow;
    [SerializeField] public KeyCode Left = KeyCode.LeftArrow;
    [SerializeField] public KeyCode Up = KeyCode.UpArrow;
    [SerializeField] public KeyCode Down = KeyCode.DownArrow;
    [SerializeField] public KeyCode hit = KeyCode.N;
    [SerializeField] public KeyCode fire = KeyCode.M;
    [SerializeField] public KeyCode slot = KeyCode.K;
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
    [SerializeField] private float FallGravity = 4.5f;

    public bool PlayerRotationRight = true;
    private bool PlayerLastRotationRight;
    private float Health;
    //jumping & falling
    private bool isGrounded;
    private bool jumped = false;
    private bool isFalling = false;
    private bool isInAir = false;
    private float startAirTime = 0f;
    [SerializeField] private float startFallTime = 1f;
    private bool knockedOut = false;
    [SerializeField] private float fallDmg=50f;

    //crouching,ladder,onewayplatform variables
    private bool isCrouching = false;
    private bool inRoll = false;
    private bool isOnLadder;
    public bool isInPlatform = false;

    // walking variables
    private float LastKeyRight = -5f;
    private float LastKeyLeft = -5f;

    //sprinting variables
    private bool sprinting = false;
    private float LastSprintRight = -5f;
    private float LastSprintLeft = -5f;

    [Header("Shooting")]
    [SerializeField] public Transform FirePoint;
    public Gun PlayerGun;
    [SerializeField] public bool shooting = false;
    private double LastTimeShoot = -5f;
    private bool ReadyToFire = false;
    private int BulletsToShot = 0;
    private Quaternion TempQuaternion = Quaternion.Euler(0, 0, 0);
    private Laser MyLaser;
    private float lastHit = 0f;

    [Header("Sounds")]
    [SerializeField] private AudioClip Jump;
    [SerializeField] private AudioClip Walk;
    [SerializeField] private AudioClip Run;
    [SerializeField] private AudioClip Reload;
    [SerializeField] private AudioClip emptySound;

    [Header("Components")]
    [SerializeField] private Rigidbody2D PlayerBody;
    [SerializeField] private BoxCollider2D PlayerHitBox;
    [SerializeField] private BoxCollider2D OpponentHitBox;
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private SpriteRenderer PlayerRenderer;
    [SerializeField] private OneWayPlatform platformScript;
    [SerializeField] private GameObject LaserPoint;
    [SerializeField] private AudioSource PlayerAudio;
    [SerializeField] private GameObject LevelManager;

    void Start()
    {
        Physics2D.IgnoreCollision(PlayerHitBox, OpponentHitBox);
        MyLaser = LaserPoint.GetComponent<Laser>();
        Health = MaxHealth;
        PlayerGun = GetGun("Pistol");
        PlayerLastRotationRight = PlayerRotationRight;
        if (!PlayerRotationRight) { transform.Rotate(0f, 180f, 0F); }
    }

    void Update()
    {
        if (LastTimeShoot + 0.5 < Time.time || !Input.GetKey(fire) && (Input.GetKey(Right) || Input.GetKey(Left) || Input.GetKey(Up) || Input.GetKey(Down) || Input.GetKey(hit) || Input.GetKey(slot)))
        {
            shooting = false; MyLaser.ShootLaser(false); PlayerRenderer.enabled = true;
        }
        isGrounded = GroundCheck();
        if ((!isGrounded) && (!isOnLadder) && (!isCrouching)) { if (!isInAir) { isInAir = true; startAirTime = Time.time; } }
        else { isInAir = false; }
        if ((((Time.time - startAirTime > startFallTime) && isInAir) || isFalling) && !knockedOut) { fall(); }
        else if (knockedOut) { PlayerBody.velocity = Vector2.zero; PlayerAudio.clip = null; startAirTime = 0f; }
        else if (isOnLadder && (!isCrouching) && (!isGrounded)) { Ladder(); PlayerAudio.clip = null; }
        else if ((!isOnLadder) && (!isCrouching) && Input.GetKey(hit)) { meleeAttack(); }
        else if (PlayerGun.name != "None" && !isCrouching && isGrounded && (Input.GetKey(fire) || shooting))
        {
            PlayerAudio.clip = null;
            shooting = true;
            PlayerRenderer.enabled = false;
            ShootPosition();
            PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y);
        }
        else
        {
            BulletsToShot = 0;
            //move
            if ((Input.GetKey(Down) && isGrounded) || isCrouching) { crouch(); PlayerAudio.clip = null; }
            else
            {
                jump();
                if (sprinting)
                {
                    sprint();
                    PlayerAudio.clip = Run;
                    if (!PlayerAudio.isPlaying) { PlayerAudio.Play(); }
                }
                else
                {
                    walk();
                    if (PlayerAudio.clip == Run) { PlayerAudio.clip = null; }
                    PlayerAudio.clip = Walk;
                }
                if (PlayerBody.velocity.y < 0)
                    PlayerBody.gravityScale = FallGravity;
                else
                    PlayerBody.gravityScale = NormalGravity;
            }
            //Fliping Player
            if (PlayerRotationRight && !PlayerLastRotationRight) { transform.Rotate(0f, 180f, 0F); }
            else if (!PlayerRotationRight && PlayerLastRotationRight) { transform.Rotate(0f, 180f, 0F); }
            PlayerLastRotationRight = PlayerRotationRight;
        }
        AnimationSetter();
        if (Health < MaxHealth && Time.time - lastHit > 5) {
            Health += 20f * Time.deltaTime;
            if (Health > MaxHealth) { Health = MaxHealth; }
        }
    }
    private void fall()
    {
        if (!isFalling) { isFalling = true; HitBoxChanger(0.5f, 0.5f, 0f, -0.575f, false); }
        else if (isGrounded)
        {
            isFalling = false;
            isInAir = false;
            TakeDamage((int)((Time.time - startAirTime + startFallTime) *fallDmg)); ;
            StartCoroutine(knockedOff());
        }
    }
    private IEnumerator knockedOff()
    {
        knockedOut = true;
        HitBoxChanger(1f, 1f, 0f, 0f, false);
        yield return new WaitForSeconds(1f);
        knockedOut = false;
        HitBoxChanger(1.2f, 2.2f, 0f, -0.075f, false);
    }
    private void meleeAttack()
    {
        
    }
    private void walk()
    {
        if (Input.GetKey(Right) && !Input.GetKey(Left)) //walk right
        {   //sprint check
            if (Input.GetKeyDown(Right) && Time.time - LastKeyRight < DoubleTapTime) { sprinting = true; }
            LastKeyRight = Time.time; PlayerRotationRight = true;
            PlayerBody.velocity = new Vector2(+WalkForce, PlayerBody.velocity.y);
            if (!PlayerAudio.isPlaying) { PlayerAudio.Play(); }
        }
        else if (Input.GetKey(Left) && !Input.GetKey(Right))     // walk left
        {   //sprint check
            if (Input.GetKeyDown(Left) && Time.time - LastKeyLeft < DoubleTapTime) { sprinting = true; }
            LastKeyLeft = Time.time; PlayerRotationRight = false;
            PlayerBody.velocity = new Vector2(-WalkForce, PlayerBody.velocity.y);
            if (!PlayerAudio.isPlaying) { PlayerAudio.Play(); }
        }
        else { PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y); PlayerAudio.clip = null; }   //stay at position
    }
    private void sprint()
    {
        if (Input.GetKey(Right) && !Input.GetKey(Left)) //sprint right
        {   //check double tap right to walk
            if (Input.GetKeyDown(Right) && Time.time - LastSprintRight < DoubleTapTime) { sprinting = false; }
            LastSprintRight = Time.time; PlayerRotationRight = true;
            PlayerBody.velocity = new Vector2(+SprintForce, PlayerBody.velocity.y);
        }
        else if (Input.GetKey(Left) && !Input.GetKey(Right))  //sprint left
        {   //check double tap left to walk
            if (Input.GetKeyDown(Left) && Time.time - LastSprintLeft < DoubleTapTime) { sprinting = false; }
            LastSprintLeft = Time.time; PlayerRotationRight = false;
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
        if (Input.GetKey(Up) && isGrounded && (!Input.GetKey(Down)) && (!isCrouching) && (!jumped))
        {
            jumped = true;
            PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, JumpForce);
            PlayerAudio.PlayOneShot(Jump);
            StartCoroutine(JumpPause());
        }
    }
    private IEnumerator JumpPause() 
    {
        yield return new WaitForSeconds(0.1f);
        jumped = false;
    }
    private void Ladder()
    {
        PlayerBody.gravityScale = 0f;
        // pohyb vpravo a vlavo
        if (Input.GetKey(Right) && !Input.GetKey(Left))
        { PlayerBody.velocity = new Vector2(+LadderHorizontal, PlayerBody.velocity.y); PlayerRotationRight = true; }
        else if (Input.GetKey(Left) && !Input.GetKey(Right))
        { PlayerBody.velocity = new Vector2(-LadderHorizontal, PlayerBody.velocity.y); PlayerRotationRight = false; }
        else { PlayerBody.velocity = new Vector2(0f, PlayerBody.velocity.y); }
        // pohyb hore a dole
        if (Input.GetKey(Up) && (!Input.GetKey(Down)))
        { PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, +LadderVertical); }
        else if (Input.GetKey(Down) && (!Input.GetKey(Up)))
        { PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, -LadderVertical); }
        else { PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, 0f); }
    }
    private void crouch()
    {
        if (Math.Abs(PlayerBody.velocity.x) <= WalkForce - 1f)
        {
            if (inRoll) 
            { 
                StopCoroutine(Roll());
                inRoll = false;
                HitBoxChanger(1.2f, 2.2f, 0f, -0.075f, false);
            }
            if (!isCrouching) { HitBoxChanger(1.2f, 1.7f, 0f, -0.323f, true); }
            else if (!Input.GetKey(Down)) { HitBoxChanger(1.2f, 2.2f, 0f, -0.075f, false); }
        }
        else if ((Math.Abs(PlayerBody.velocity.x) <= WalkForce) && (!inRoll)) { StartCoroutine(Roll()); }
        else if (((Math.Abs(PlayerBody.velocity.x) <= SprintForce) && (Math.Abs(PlayerBody.velocity.x) >= WalkForce)) && (!inRoll))
        {
            if (!isCrouching)
            {
                HitBoxChanger(2f, 1f, 0f, 0f, true);
                PlayerBody.gravityScale = ThrowJumpGravity;
                PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, ThrowJump);
                PlayerAudio.PlayOneShot(Jump);
            }
            else if (isGrounded)
            {
                if (PlayerRotationRight) { PlayerBody.velocity = new Vector2(WalkForce, 0f); }
                else if (!PlayerRotationRight) { PlayerBody.velocity = new Vector2(-WalkForce, 0f); }
                PlayerBody.gravityScale = NormalGravity;
                StartCoroutine(Roll());
            }
        }
    }
    private IEnumerator Roll()
    {
        inRoll = true;
        HitBoxChanger(1.2f, 1.2f, 0f, -0.575f, true);
        yield return new WaitForSeconds(TimeInRoll);
        HitBoxChanger(1.2f, 2.2f, 0f, -0.075f, false);
        yield return new WaitForSeconds(0.1f);
        inRoll = false;
    }
    private void HitBoxChanger(float sizeX, float sizeY, float offsetX, float offsetY, bool isCrouching)
    {
        PlayerHitBox.size = new Vector2(sizeX, sizeY);
        PlayerHitBox.offset = new Vector2(offsetX, offsetY);
        this.isCrouching = isCrouching;
    }
    public float getHealt
    {
        get { return Health; }
        set { }
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
                        BulletsToShot -= 1; PlayerGun.ammo -= 1; LastTimeShoot = Time.time; PlayerAudio.PlayOneShot(PlayerGun.Sound);
                    }
                    break;
                case "AssalutRifle":
                    if (Time.time > LastTimeShoot + 0.1f){
                        Instantiate(PlayerGun.Bullet, FirePoint.position, FirePoint.rotation);
                        BulletsToShot -= 1; PlayerGun.ammo -= 1; LastTimeShoot = Time.time; PlayerAudio.PlayOneShot(PlayerGun.Sound);
                    }
                    break;
            }
            
            return;
        }
        if(PlayerGun.name is "SniperRifle") { MyLaser.ShootLaser(true); }
        else { MyLaser.ShootLaser(false); }
        if (Input.GetKeyDown(hit) || Input.GetKeyDown(slot)) { shooting = false; MyLaser.ShootLaser(false); }
        if (Input.GetKey(fire)) { ReadyToFire = true; }    
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
                        PlayerAudio.PlayOneShot(PlayerGun.Sound);
                        PlayerAudio.PlayOneShot(Reload);
                        break;
                    case "Mac-10":
                    case "AssalutRifle":
                        BulletsToShot =  PlayerGun.ammo >= PlayerGun.BulletsOnShoot ? PlayerGun.BulletsOnShoot : PlayerGun.ammo;
                        break;
                    default:
                        Instantiate(PlayerGun.Bullet, FirePoint.position, FirePoint.rotation);
                        PlayerAudio.PlayOneShot(PlayerGun.Sound);
                        PlayerGun.ammo -= 1;
                        if(PlayerGun.name == "SniperRifle") { PlayerAudio.PlayOneShot(Reload); }
                        break;
                }
            }
        }
        if (Input.GetKey(fire)) { LastTimeShoot = Time.time; }
        if (PlayerGun.ammo <= 0){ PlayerAudio.PlayOneShot(emptySound); PlayerGun = GetGun("None"); }
    }
    private bool GroundCheck()
    {
        RaycastHit2D rayCastHit1 = Physics2D.Raycast(PlayerHitBox.bounds.center, Vector2.down, PlayerHitBox.bounds.extents.y - 0.2f, OneWayPlatformLayerMask);
        RaycastHit2D rayCastHit2 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x + PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y), Vector2.down, PlayerHitBox.bounds.extents.y - 0.2f, OneWayPlatformLayerMask);
        RaycastHit2D rayCastHit3 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x - PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y), Vector2.down, PlayerHitBox.bounds.extents.y - 0.2f, OneWayPlatformLayerMask);
        if (isInPlatform)
            return false;
        else if (rayCastHit1.collider != null || rayCastHit2.collider != null || rayCastHit3.collider != null)
        {
            platformScript.currentPlatform = null;
            StartCoroutine(IsInPlatform());
            return false;
        }
        else
        {
            rayCastHit1 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, OneWayPlatformLayerMask);
            rayCastHit2 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x + PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, OneWayPlatformLayerMask);
            rayCastHit3 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x - PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, OneWayPlatformLayerMask);
            if (rayCastHit1.collider != null) { platformScript.currentPlatform = rayCastHit1.collider.gameObject; return true; }
            else if (rayCastHit2.collider != null) { platformScript.currentPlatform = rayCastHit2.collider.gameObject; return true; }
            else if (rayCastHit3.collider != null) { platformScript.currentPlatform = rayCastHit3.collider.gameObject; return true; }
            else
            {
                platformScript.currentPlatform = null;
                rayCastHit1 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, PlatformLayerMask);
                rayCastHit2 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x + PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, PlatformLayerMask);
                rayCastHit3 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x - PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, PlatformLayerMask);
                if ((rayCastHit1.collider != null || rayCastHit2.collider != null || rayCastHit3.collider != null))
                    return true;
                else return false;
            }
        }
    }
    private IEnumerator IsInPlatform()
    {
        isInPlatform = true;
        yield return new WaitForSeconds(0.4f);
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
        lastHit = Time.time;
        Health -= damage;
        if (Health <= 0) { death(); }
    }
    private void AnimationSetter()
    {
        if((isGrounded && Input.GetKey(Up)) || !isGrounded || isInPlatform)
            PlayerAnimator.SetBool("isGrounded", false);
        else
            PlayerAnimator.SetBool("isGrounded", true);
        PlayerAnimator.SetBool("isCrouching", isCrouching);
        PlayerAnimator.SetBool("isOnLadder", isOnLadder);
        PlayerAnimator.SetFloat("PlayerSpeed", Math.Abs(PlayerBody.velocity.x));
        if (PlayerBody.velocity != Vector2.zero)
            PlayerAnimator.SetBool("isLadderMoving", true);
        else
            PlayerAnimator.SetBool("isLadderMoving", false);    
    }
    private Gun GetGun(string name)
    {
        GameObject GM = GameObject.Find("LevelManager");
        GunManager GunM = GM.GetComponent<GunManager>();
        foreach (var Gunitem in GunM.AllGuns){
            if (name == Gunitem.name) { 
                Gun TempGun = Gunitem.Clone();
                GameObject TempBullet = PlayerName == "Player_1" ? TempGun.Bullet : TempGun.Bullet2P;
                TempBullet.GetComponent<bullet>().damage = TempGun.damage;
                TempBullet.GetComponent<bullet>().speed = TempGun.speed;
                TempGun.Bullet = TempBullet;
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
    public bool PickUpGun(string GunName)
    {
        if (GunName == "MedicKit") { Health = MaxHealth; return true; }
        if(PlayerGun.name == "None") { PlayerGun = GetGun(GunName); return true; }
        else if (Input.GetKey(Down) && Input.GetKey(hit)) { PlayerGun = GetGun(GunName); return true; }
        return false;
    }
    private void death()
    {
        LevelManager.GetComponent<sceneManager>().setEndScreen(name);
        Destroy(gameObject);
    }
}
