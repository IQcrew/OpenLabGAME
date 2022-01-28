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
    [Header("Health & Saturation")]
    [SerializeField] public string PlayerName;
    [SerializeField] public int MaxHealth = 200;
    [SerializeField] public int Saturation = 100;

    public bool PlayerRotationRight = true;
    private bool PlayerLastRotationRight;
    private float Health;
    // movement
    private bool GoRight;
    private bool GoLeft;
    private bool GoUp;
    private bool GoDown;

    //jumping & falling
    private bool isGrounded;
    private float jumpTime = 0f;
    private bool isFalling = false;
    private float maxFallSpeed = 0f;
    private bool knockedOut = false;
    private float fallingTime;

    //crouching,ladder,onewayplatform variables
    private bool isCrouching = false;
    private bool inRoll = false;
    private bool isOnLadder;
    public bool isInPlatform = false;
    private float lastExplosion = -10;

    // walking variables
    private float LastKeyRight = -5f;
    private float LastKeyLeft = -5f;

    //sprinting variables
    private bool sprinting = false;
    private float LastSprintRight = -5f;
    private float LastSprintLeft = -5f;
    [Header("LayerMasks")]
    [SerializeField] public LayerMask platformLayerMask;
    [SerializeField] public LayerMask oneWayPlatformLayerMask;
    [SerializeField] public LayerMask playerLayerMask;
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

    [Header("Fighting")]
    private float lastHitMelee = 0f;
    [SerializeField] private float kickForce;
    private bool kicked = false;
    private MeleeWeapon PlayerWeapon;

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
    private FirePoint FP;
    private granadePack PlayerGranade;
    

    private float WalkForce = 5f;
    private float SprintForce = 8f;
    private float JumpForce = 10f;
    private float LadderVertical = 5f;
    private float LadderHorizontal = 3f;
    private float DoubleTapTime = 0.2f;
    private float TimeInRoll = 0.5f;
    private float timeBetweenRoll = 0.2f;
    private float ThrowJump = 5f;
    private float ThrowJumpGravity = 2f;
    private float NormalGravity = 2.5f;
    private float FallGravity = 4.5f;
    private float fallSpeed = 15f;
    private float fallAcceleration = 20f;
    private float fallDmg = 2f;
    private AudioClip Jump;
    private AudioClip Walk;
    private AudioClip Run;
    private AudioClip Reload;
    private AudioClip emptySound;
    private AudioClip getHit;
    private AudioClip deathSound;

    void Start()
    {
        FP = FirePoint.GetComponent<FirePoint>();
        playerTemplate PT = GameObject.Find("LevelManager").GetComponent<playerTemplate>();
        GunManager GM = GameObject.Find("LevelManager").GetComponent<GunManager>();
        PlayerWeapon = GM.AllMeleeWeapons[0];
        //setup values
        WalkForce = PT.WalkForce;
        SprintForce = PT.SprintForce;
        JumpForce = PT.JumpForce;
        LadderVertical = PT.LadderVertical;
        LadderHorizontal = PT.LadderHorizontal;
        DoubleTapTime = PT.DoubleTapTime;
        TimeInRoll = PT.TimeInRoll;
        ThrowJump = PT.ThrowJump;
        ThrowJumpGravity = PT.ThrowJumpGravity;
        NormalGravity = PT.NormalGravity;
        FallGravity = PT.FallGravity;
        fallSpeed = PT.fallSpeed;
        fallAcceleration = PT.fallAcceleration;
        fallDmg = PT.fallDmg;
        Jump = PT.Jump;
        Walk = PT.Walk;
        Run = PT.Run;
        Reload = PT.Reload;
        emptySound = PT.emptySound;
        deathSound = PT.deathSound;
        getHit = PT.getHit;

        Physics2D.IgnoreCollision(PlayerHitBox, OpponentHitBox);
        MyLaser = LaserPoint.GetComponent<Laser>();
        Health = MaxHealth;
        PlayerGun = GetGun("Pistol");
        PlayerGranade = GetGranade("None");
        PlayerLastRotationRight = PlayerRotationRight;
        if (!PlayerRotationRight) { transform.Rotate(0f, 180f, 0F); }
    }

    void Update()
    {
        if (Input.GetKey(Right) && !Input.GetKey(Left)) { GoRight = true; GoLeft = false; }
        else if ((!Input.GetKey(Right)) && Input.GetKey(Left)) { GoRight = false; GoLeft = true; }
        else { GoRight = false; GoLeft = false; }
        if (Input.GetKey(Up) && !Input.GetKey(Down)) { GoUp = true; GoDown = false; }
        else if ((!Input.GetKey(Up)) && Input.GetKey(Down)) { GoUp = false; GoDown = true; }
        else { GoUp = false; GoDown = false; }

        if (LastTimeShoot + 0.5 < Time.time || !Input.GetKey(fire) && (GoRight || GoLeft || GoUp || GoDown || Input.GetKey(hit) || Input.GetKey(slot)))
        {
            shooting = false; MyLaser.ShootLaser(false); PlayerRenderer.enabled = true; FP.exitFP();
        }

        isGrounded = GroundCheck();

        if (isGrounded) { kicked = false; }
        if (isCrouching) { crouch(); PlayerAudio.clip = null; }
        else if (PlayerBody.velocity.y <= -fallSpeed || isFalling) { fall(); }
        else if (knockedOut) { PlayerBody.velocity = Vector2.zero; PlayerAudio.clip = null; }
        else if (isOnLadder && (!isGrounded)) { Ladder(); PlayerAudio.clip = null; }
        else if (Input.GetKey(hit)) { meleeAttack(); }
        else if (PlayerGun.name != "None" && isGrounded && (Input.GetKey(fire) || shooting))
        {
            if (GoRight) { PlayerRotationRight = true; }
            else if (GoLeft) { PlayerRotationRight = false; }
            if (PlayerRotationRight && !PlayerLastRotationRight) { transform.Rotate(0f, 180f, 0F); }
            else if (!PlayerRotationRight && PlayerLastRotationRight) { transform.Rotate(0f, 180f, 0F); }
            PlayerLastRotationRight = PlayerRotationRight;
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
            if (GoDown && isGrounded) { crouch(); PlayerAudio.clip = null; }
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
        if (PlayerBody.velocity.y < maxFallSpeed) { maxFallSpeed = PlayerBody.velocity.y; }
        if (!isFalling) { isFalling = true; fallingTime = Time.time; HitBoxChanger(0.5f, 0.5f, 0f, -0.575f, false); }
        else if (isGrounded && !(lastExplosion + 0.1 > Time.time))
        {
            isFalling = false;
            TakeDamage((int)(fallDmg * Math.Abs(maxFallSpeed - (Time.time - fallingTime) * fallAcceleration)));
            PlayerBody.velocity = Vector2.zero;
            StartCoroutine(knockedOff());
        }
    }
    public void giveExplosion(int dmg)
    {
        TakeDamage(dmg);
        lastExplosion = Time.time;
        isFalling = true;
        fallingTime = Time.time;
    }
    private IEnumerator knockedOff()
    {
        knockedOut = true;
        HitBoxChanger(1f, 1f, 0f, 0f, false);
        yield return new WaitForSeconds(1f);
        knockedOut = false;
        HitBoxChanger(1.2f, 2.2f, 0f, -0.075f, false);
        maxFallSpeed = 0f;
    }
    private void meleeAttack()
    {
        if (isGrounded)
        {
            PlayerBody.velocity = Vector2.zero;
            PlayerAudio.clip = null;
            RaycastHit2D rayCastHit;
            if (PlayerRotationRight) { rayCastHit = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x + PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y), Vector2.right, PlayerHitBox.bounds.size.x, playerLayerMask); }
            else { rayCastHit = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x - PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y), Vector2.left, PlayerHitBox.bounds.size.x, playerLayerMask); }
            if (rayCastHit.collider != null && Time.time - lastHitMelee > PlayerWeapon.hitSpeed)
            {
                rayCastHit.collider.GetComponent<Player>().TakeDamage(PlayerWeapon.damage);
                lastHitMelee = Time.time;
            }
        }
        else
        {
            RaycastHit2D rayCastHit;
            if (PlayerRotationRight) { rayCastHit = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x + PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y-PlayerHitBox.bounds.extents.y), Vector2.right, PlayerHitBox.bounds.size.x, playerLayerMask); }
            else { rayCastHit = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x - PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.left, PlayerHitBox.bounds.size.x, playerLayerMask); }
            if (rayCastHit.collider != null && !kicked)
            {
                rayCastHit.collider.GetComponent<Player>().giveExplosion(20);
                rayCastHit.collider.GetComponent<Rigidbody2D>().AddForce(PlayerBody.velocity * kickForce);
                kicked = true;
            }
        }
    }
    private void walk()
    {
        if (GoRight) //walk right
        {   //sprint check
            if (Input.GetKeyDown(Right) && Time.time - LastKeyRight < DoubleTapTime) { sprinting = true; }
            LastKeyRight = Time.time; PlayerRotationRight = true;
            PlayerBody.velocity = new Vector2(+WalkForce, PlayerBody.velocity.y);
            if (!PlayerAudio.isPlaying) { PlayerAudio.Play(); }
        }
        else if (GoLeft)     // walk left
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
        if (GoRight) //sprint right
        {   //check double tap right to walk
            if (Input.GetKeyDown(Right) && Time.time - LastSprintRight < DoubleTapTime) { sprinting = false; }
            LastSprintRight = Time.time; PlayerRotationRight = true;
            PlayerBody.velocity = new Vector2(+SprintForce, PlayerBody.velocity.y);
        }
        else if (GoLeft)  //sprint left
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
        if (GoUp && isGrounded && (!isCrouching))
        {
            if (Time.time - jumpTime > 0.1f)
            {
                PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, JumpForce);
                PlayerAudio.PlayOneShot(Jump);
                jumpTime = Time.time;
            }
        }
    }
    private void Ladder()
    {
        PlayerBody.gravityScale = 0f;
        // pohyb vpravo a vlavo
        if (GoRight)
        { PlayerBody.velocity = new Vector2(+LadderHorizontal, PlayerBody.velocity.y); PlayerRotationRight = true; }
        else if (GoLeft)
        { PlayerBody.velocity = new Vector2(-LadderHorizontal, PlayerBody.velocity.y); PlayerRotationRight = false; }
        else { PlayerBody.velocity = new Vector2(0f, PlayerBody.velocity.y); }
        // pohyb hore a dole
        if (GoUp)
        { PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, +LadderVertical); }
        else if (GoDown)
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
            else if (!GoDown) { HitBoxChanger(1.2f, 2.2f, 0f, -0.075f, false); }
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
        yield return new WaitForSeconds(timeBetweenRoll);
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
        FP.FUpdate();
        if (BulletsToShot > 0) {
            switch (PlayerGun.name)
            {
                case "Mac-10":
                    if (Time.time > LastTimeShoot + 0.08f) {
                        TempQuaternion = Quaternion.Euler(0, 0, (((float)rrr.NextDouble()) * 10) - 5);
                        shootingBullet(QuaternionDifference(TempQuaternion, FirePoint.rotation));
                        BulletsToShot -= 1; PlayerGun.ammo -= 1; LastTimeShoot = Time.time; PlayerAudio.PlayOneShot(PlayerGun.Sound, PlayerGun.fireVolume);
                    }
                    break;
                case "AssalutRifle":
                    if (Time.time > LastTimeShoot + 0.1f) {

                        shootingBullet(FirePoint.rotation);
                        BulletsToShot -= 1; PlayerGun.ammo -= 1; LastTimeShoot = Time.time; PlayerAudio.PlayOneShot(PlayerGun.Sound, PlayerGun.fireVolume);
                    }
                    break;
            }
            return;
        }
        if (PlayerGun.name is "SniperRifle") { MyLaser.ShootLaser(true); }
        else { MyLaser.ShootLaser(false); }
        if (Input.GetKeyDown(hit) || Input.GetKeyDown(slot)) { shooting = false; MyLaser.ShootLaser(false); }
        if (Input.GetKey(fire)) { ReadyToFire = true; }
        else if (ReadyToFire && PlayerGun.name != "" && PlayerGun.name != "None")
        {
            ReadyToFire = false;
            if (PlayerGun.fire()) {
                switch (PlayerGun.name) {
                    case "Shotgun":
                        for (int i = 0; i < 4; i++) {
                            TempQuaternion = Quaternion.Euler(0, 0, (((float)rrr.NextDouble()) * 14) - 7);
                            shootingBullet(QuaternionDifference(TempQuaternion, FirePoint.rotation));
                        }
                        PlayerGun.ammo -= 1;
                        PlayerAudio.PlayOneShot(PlayerGun.Sound, PlayerGun.fireVolume);
                        PlayerAudio.PlayOneShot(Reload, PlayerGun.fireVolume);
                        break;
                    case "Mac-10":
                    case "AssalutRifle":
                        BulletsToShot = PlayerGun.ammo >= PlayerGun.BulletsOnShoot ? PlayerGun.BulletsOnShoot : PlayerGun.ammo;
                        break;
                    default:
                        shootingBullet(FirePoint.rotation);
                        PlayerAudio.PlayOneShot(PlayerGun.Sound, PlayerGun.fireVolume);
                        PlayerGun.ammo -= 1;
                        if (PlayerGun.name == "SniperRifle") { PlayerAudio.PlayOneShot(Reload, PlayerGun.fireVolume); }
                        break;
                }
            }
        }
        if (Input.GetKey(fire)) { LastTimeShoot = Time.time; }
        if (PlayerGun.ammo <= 0) { PlayerAudio.PlayOneShot(emptySound); PlayerGun = GetGun("None"); }
    }
    private void GranadePosition() //treba dorobit
    {
        FP.FUpdate();
    }

    private void shootingBullet(Quaternion rotation)
    {
        GameObject TVP = Instantiate(PlayerGun.Bullet, FirePoint.position, rotation);
        TVP.GetComponent<bullet>().shooter_name = PlayerName;
        TVP.GetComponent<bullet>().damage = PlayerGun.damage;
        TVP.GetComponent<bullet>().speed = PlayerGun.speed;
    }


    private bool GroundCheck()
    {
        RaycastHit2D rayCastHit1 = Physics2D.Raycast(PlayerHitBox.bounds.center, Vector2.down, PlayerHitBox.bounds.extents.y - 0.2f, oneWayPlatformLayerMask);
        RaycastHit2D rayCastHit2 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x + PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y), Vector2.down, PlayerHitBox.bounds.extents.y - 0.2f, oneWayPlatformLayerMask);
        RaycastHit2D rayCastHit3 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x - PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y), Vector2.down, PlayerHitBox.bounds.extents.y - 0.2f, oneWayPlatformLayerMask);
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
            rayCastHit1 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, oneWayPlatformLayerMask);
            rayCastHit2 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x + PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, oneWayPlatformLayerMask);
            rayCastHit3 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x - PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, oneWayPlatformLayerMask);
            if (rayCastHit1.collider != null) { platformScript.currentPlatform = rayCastHit1.collider.gameObject; return true; }
            else if (rayCastHit2.collider != null) { platformScript.currentPlatform = rayCastHit2.collider.gameObject; return true; }
            else if (rayCastHit3.collider != null) { platformScript.currentPlatform = rayCastHit3.collider.gameObject; return true; }
            else
            {
                platformScript.currentPlatform = null;
                rayCastHit1 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, platformLayerMask);
                rayCastHit2 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x + PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, platformLayerMask);
                rayCastHit3 = Physics2D.Raycast(new Vector2(PlayerHitBox.bounds.center.x - PlayerHitBox.bounds.extents.x, PlayerHitBox.bounds.center.y - PlayerHitBox.bounds.extents.y), Vector2.down, 0.1f, platformLayerMask);
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
        if (Health <= 0f) { death(); }
        PlayerAudio.PlayOneShot(getHit);
    }
    private void AnimationSetter()
    {
        if ((isGrounded && GoUp) || !isGrounded || isInPlatform)
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
    private Quaternion QuaternionDifference(Quaternion origin, Quaternion target)
    {
        Quaternion identityOrigin = Quaternion.identity * Quaternion.Inverse(origin);
        Quaternion identityTarget = Quaternion.identity * Quaternion.Inverse(target);
        return identityOrigin * Quaternion.Inverse(identityTarget);
    }
    private Gun GetGun(string name)
    {
        GunManager GunM = GameObject.Find("LevelManager").GetComponent<GunManager>();
        foreach (var Gunitem in GunM.AllGuns) { if (name == Gunitem.name) { return Gunitem.Clone(); } }
        return GunM.AllGuns[0];
    }
    private granadePack GetGranade(string name)
    {
        GunManager GunM = GameObject.Find("LevelManager").GetComponent<GunManager>();
        foreach (var Grnde in GunM.AllGranades){if (name == Grnde.name) { return Grnde.Clone(); }}
        return GunM.AllGranades[0];
    }
    private MeleeWeapon GetMelee(string name)
    {
        GunManager GunM = GameObject.Find("LevelManager").GetComponent<GunManager>();
        foreach (var melleW in GunM.AllMeleeWeapons) { if (name == melleW.name) { return melleW.Clone(); } }
        return GunM.AllMeleeWeapons[0];
    }
    public bool PickUpWeapon(string WeaponName, string type)
    {
        switch (type)
        {
            case "Gun":
                if (WeaponName == "MedicKit") { Health = MaxHealth; return true; }
                if (PlayerGun.name == "None") { PlayerGun = GetGun(WeaponName); return true; }
                else if (isCrouching && Input.GetKey(hit)) { PlayerGun = GetGun(WeaponName); return true; }
                return false;
            case "Melee":
                if (PlayerWeapon.name == "Hand") { PlayerWeapon = GetMelee(WeaponName); return true; }
                else if (isCrouching && Input.GetKey(hit)) { PlayerWeapon = GetMelee(WeaponName); return true; }
                return false;
            case "Granade":
                if (PlayerGranade.name == "None") { PlayerGranade = GetGranade(WeaponName); return true; }
                else if (isCrouching && Input.GetKey(hit)) { PlayerGranade = GetGranade(WeaponName); return true; }
                return false;
            default:
                return false;
        }
        
    }
    private void death()
    {
        GameObject.Find("LevelManager").GetComponent<AudioSource>().PlayOneShot(deathSound);
        LevelManager.GetComponent<sceneManager>().setEndScreen(name);
        Destroy(gameObject);
    }
}
