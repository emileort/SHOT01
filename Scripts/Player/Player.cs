using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [SerializeField] StatsBar_HUD statsBar_HUD;

    [SerializeField] bool regenerateHealth = true;

    [SerializeField] float healthRegenerateTime;

    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;

    [Header("----INPUT----")]

    [SerializeField] PlayerInput input;

    [Header("----MOVE----")]

    [SerializeField] float moveSpeed = 10f;

    [SerializeField] float accelerationTime = 3f;

    [SerializeField] float decelerationTime = 3f;

    [SerializeField] float moveRotationAngle = 50f;

    [SerializeField] float paddingX = 0.2f;

    [SerializeField] float paddingY = 0.2f;

    [Header("----FIRE----")]

    [SerializeField] GameObject projectile1;

    [SerializeField] GameObject projectile2;

    [SerializeField] GameObject projectile3;

    [SerializeField] GameObject projectile4;

    [SerializeField] GameObject projectile5;

    [SerializeField] GameObject projectile6;

    [SerializeField] GameObject projectile7;

    [SerializeField] GameObject projectileOverdrive;

    [SerializeField] ParticleSystem muzzleVFX;

    [SerializeField] Transform muzzleMiddle;

    [SerializeField] Transform muzzleTop;

    [SerializeField] Transform muzzleBottom;

    [SerializeField] Transform muzzleMID;

    [SerializeField] Transform muzzleBackMIDBotton;

    [SerializeField] Transform muzzleBackMIDTop;

    [SerializeField] Transform muzzleBackBottom;

    [SerializeField] Transform muzzleBackTop;

    [SerializeField] AudioData projectileLaunchSFX;

    [SerializeField, Range(0, 4)] int weaponPower = 0;

    [SerializeField] float fireInterval = 0.2f;

    [Header("----DODGE----")]

    [SerializeField] AudioData dodgeSFX;

    [SerializeField,Range(0,100)] int dodgeEnergyCost = 25;

    [SerializeField] float maxRoll = 720f;

    [SerializeField] float rollSpeed = 360f;

    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);

    [Header("----OVERDRIVE----")]

    [SerializeField] int overdriveDodgeFactor = 2;

    [SerializeField] float overdriveSpeedFactor = 1.2f;

    [SerializeField] float overdriveFireFactor = 1.2f;

    public GameObject thatOneGuy2;

    public GameObject thatOneGuy3;

    bool isDodging = false;

    bool isOverdriving = false;

    public bool IsFullHealth => health == maxHealth;

    public bool IsFullPower => weaponPower == 4;


    readonly float SlowMotionDuration = 1f;

    readonly float InvincibleTime = 0.2f;



    float currentRoll;

    float dodgeDuration;

    Vector2 moveDirection;

    Vector2 previousVelocity;

    Quaternion previousRotation;

    new Rigidbody2D rigidbody;

    Coroutine moveCoroutine;

    Coroutine healthRegenerateCoroutine;

    WaitForSeconds waitForFireInterval;

    WaitForSeconds waitForOverdriveFireInterval;

    WaitForSeconds waitHealthRegenerateTime;

    WaitForSeconds waitDecelerationTime;

    WaitForSeconds waitInvivcibleTime;
    
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    Animator animator;

    Animator animator1;

    public AnimatorHandler animatorHandler;

    new Collider2D collider;

    MissileSystem missile;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        missile = GetComponent<MissileSystem>();

        dodgeDuration = maxRoll / rollSpeed;

        animator = GetComponent<Animator>();

        animatorHandler = GetComponentInChildren<AnimatorHandler>();

        rigidbody.gravityScale = 0f;

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval /= overdriveFireFactor);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitDecelerationTime = new WaitForSeconds(decelerationTime);
        waitInvivcibleTime = new WaitForSeconds(InvincibleTime);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverdrive += Overdrive;
        input.onLaunchMissile += LaunchMissile;
        input.onShift += Shift;
        input.onShiftStop += ShiftStop;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }

    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverdrive -= Overdrive;
        input.onLaunchMissile -= LaunchMissile;
        input.onShift -= Shift;
        input.onShiftStop -= ShiftStop;

        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }

    // Start is called before the first frame update
    void Start()
    {

        statsBar_HUD.Initialize(health, maxHealth);
        
        thatOneGuy3.SetActive(false);

        input.EnableGameplayInput();

    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        BloodScreen.Instance.Show();

        GameObject.Find("Main Camera").GetComponent<CameraShake>().isShake = true;

        Animator animator = thatOneGuy2.GetComponent<Animator>();

        Animator animator1 = thatOneGuy3.GetComponent<Animator>();

        

        if (animator != null && health > maxHealth / 2)
        {
            animator.Play("hurt");

        }

        else if (animator1 != null && health <= maxHealth / 2)
        {
            thatOneGuy2.SetActive(false);
            thatOneGuy3.SetActive(true);
            animator1.Play("hurt1");
        }




        statsBar_HUD.UpdateStats(health, maxHealth);

        TimeController.Instance.BulletTime(SlowMotionDuration);


        if (gameObject.activeSelf)
        {
            Move(moveDirection);

            StartCoroutine(InvivcibleCorountine());

            if (regenerateHealth)
            {
                if (healthRegenerateCoroutine != null)
                {
                    StopCoroutine(healthRegenerateCoroutine);
                }

                healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        statsBar_HUD.UpdateStats(health, maxHealth);
    }

    public override void Die()
    {
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        statsBar_HUD.UpdateStats(0f, maxHealth);
        base.Die();
    }

    IEnumerator InvivcibleCorountine()
    {
        collider.isTrigger = true;

        yield return waitInvivcibleTime;

        collider.isTrigger = false;
    }

    #region MOVE
    // Update is called once per frame
    void Move(Vector2 moveInput)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);

        Animator animator = thatOneGuy2.GetComponent<Animator>();

        Animator animator1 = thatOneGuy3.GetComponent<Animator>();

        var keyboard = Keyboard.current;

        if (keyboard.dKey.wasPressedThisFrame)
        {
            if (animator != null && health > maxHealth / 2)
            {
                animator.Play("Forward");
            }
            else if (animator1 != null && health <= maxHealth / 2)
            {
                thatOneGuy2.SetActive(false);
                thatOneGuy3.SetActive(true);
                animator1.Play("Forward1");
            }
        }


        //  if (keyboard.dKey.wasReleasedThisFrame)


        moveDirection = moveInput.normalized;
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveDirection * moveSpeed, moveRotation));
        StopCoroutine(nameof(DecelerationCoroutine));
        StartCoroutine(nameof(MovePositionLimitCoroutine));
    }

    void StopMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveDirection = Vector2.zero;
        moveCoroutine=StartCoroutine(MoveCoroutine(decelerationTime, moveDirection, Quaternion.identity));
        StartCoroutine(nameof(DecelerationCoroutine));
    }

    IEnumerator MoveCoroutine(float time,Vector2 moveVelocity,Quaternion moveRotation)
    {
        float t = 0f;

        previousVelocity = rigidbody.velocity;
        previousRotation = transform.rotation;

        while (t < 1f)
        {
            t += Time.fixedDeltaTime / time;
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t );
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t );

            yield return waitForFixedUpdate;
        }
    }

    

    IEnumerator MovePositionLimitCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);

            yield return waitForFixedUpdate;
        }
    }

    IEnumerator DecelerationCoroutine()
    {
        yield return waitDecelerationTime;

        StopCoroutine(nameof(MovePositionLimitCoroutine));
    }


    #endregion

    #region FIRE

    void Fire()
    {
        muzzleVFX.Play();
        StartCoroutine(nameof(FireCoroutine));
    }

    void StopFire()
    {
        muzzleVFX.Stop();
        StopCoroutine(nameof(FireCoroutine));
    }

    IEnumerator FireCoroutine()
    {
        //(聲明開火間隔變量)WaitForSeconds waitForFireInterval = new WaitForSeconds(fireInterval);
        //循環
        while (true)
        {
            
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile3, muzzleBottom.position);
                    break;
                case 2:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile3, muzzleBottom.position);
                    break;
                case 3:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile3, muzzleBottom.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile7, muzzleBackMIDBotton.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile6, muzzleBackMIDTop.position);
                    break;
                case 4:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile5, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile4, muzzleBottom.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile4, muzzleBackBottom.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile5, muzzleBackTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile6, muzzleBackMIDTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile7, muzzleBackMIDBotton.position);
                    break;
                default:
                    break;
            }


            AudioManager.Instance.PlaySFX(projectileLaunchSFX);

            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;

            //if (isOverdriving)
            //{
            //    yield return waitForOverdriveFireInterval;
            //}
            //else
            //{
            //    yield return waitForFireInterval;
            //}

        }

        // return isOverdriving ? projectileOverdrive : projectile1; 三元判斷式

    }

    #endregion

    #region DODGE
    
    void Dodge()
    {
        if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
        
        StartCoroutine(nameof(DodgeCoroutine));
        //改變位置
    }

    IEnumerator DodgeCoroutine()
    {
        isDodging = true ;

        AudioManager.Instance.PlayRandomSFX(dodgeSFX);
        //消耗能量
        PlayerEnergy.Instance.Use(dodgeEnergyCost);

        //閃避無敵(改變觸發器開關)
        collider.isTrigger = true;

        //動作閃躲(閃避角度)
        currentRoll = 0f;

        TimeController.Instance.BulletTime(SlowMotionDuration, SlowMotionDuration);

        var scale = transform.localScale;

        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

            if (currentRoll < maxRoll / 2f)
            {
                scale.x = Mathf.Clamp(scale.x - Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
                scale.y = Mathf.Clamp(scale.y - Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
                scale.z = Mathf.Clamp(scale.z - Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
            }

            else
            {
                //scale += (Time.deltaTime / dodgeDuration) * Vector3.one;
                scale.x = Mathf.Clamp(scale.x + Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
                scale.y = Mathf.Clamp(scale.y + Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
                scale.z = Mathf.Clamp(scale.z + Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
            }

            transform.localScale = scale;

            yield return waitForFixedUpdate;
        }


        collider.isTrigger = false;
        isDodging = false;
    }
    #endregion

    #region OVERDRIVE

    void Overdrive()
    {
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;

        PlayerOverdrive.on.Invoke();
    }

    void OverdriveOn()
    {
        isOverdriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;
        moveSpeed *= overdriveSpeedFactor;

        TimeController.Instance.BulletTime(SlowMotionDuration, SlowMotionDuration);
    }

    void OverdriveOff()
    {
        isOverdriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
    }

    #endregion

    void Shift()
    {
        moveSpeed = moveSpeed/2;
    }

    void ShiftStop()
    {
        moveSpeed = moveSpeed*2;
    }

    void LaunchMissile()
    {
        missile.Launch(muzzleMID);
    }

    public void PickUpMissile()
    {
        missile.PickUp();
    }

    public void PowerUp()
    {
        // weaponPower += 1;
        
        // weaponPower++;
        
        // weaponPower = Mathf.Clamp(weaponPower, 0, 2);

        weaponPower = Mathf.Min(weaponPower + 1, 2, 3, 4);
    }

    public void PowerDown()
    {
        // weaponPower--;
        // weaponPower = Mathf.Clamp(weaponPower, 0, 2);

        // weaponPower = Mathf.Max(weaponPower - 1, 0);

        // weaponPower = Mathf.Clamp(weaponPower, --weaponPower, 0);

        weaponPower = Mathf.Max(--weaponPower, 0);
    }
}
