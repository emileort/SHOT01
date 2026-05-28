using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    [SerializeField] float continuousFireDuration = 1.5f;

    [Header("----PLAYER DETECTION----")]

    [SerializeField] Transform playerDetectionTransform;

    [SerializeField] Vector3 playerDetectionSize;

    [SerializeField] LayerMask playerLayer;

    [Header("----BEAM----")]

    [SerializeField] float beamCooldownTime = 12f;

    [SerializeField] AudioData beamChargingSFX;

    [SerializeField] AudioData beamLaunchSFX;

    public GameObject Boss;

    bool isBeamReady;

    int launchBeamID = Animator.StringToHash("launchBeam");

    WaitForSeconds waitForCoutinuousFireInterval;

    WaitForSeconds waitForFireInterval;

    WaitForSeconds waitBeamCooldownTime;

    List<GameObject> magazine;

    AudioData launchSFX;

    Animator animator;

    Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;

        animator = GetComponent<Animator>();

        waitForCoutinuousFireInterval = new WaitForSeconds(minFireInterval);

        waitForFireInterval = new WaitForSeconds(maxFireInterval);

        magazine = new List<GameObject>(projectiles.Length);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //

    }

    public void Start()
    {
        Animator animator = Boss.GetComponent<Animator>();
        animator.Play("Boss open");
    }

    protected override void OnEnable()
    {
        isBeamReady = false;
        StartCoroutine(nameof(BeamCooldownCoroutine));
        base.OnEnable();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerDetectionTransform.position, playerDetectionSize);
    }

    void ActivateBeamWeapon()
    {
        isBeamReady = false;

        animator.SetTrigger(launchBeamID);

        AudioManager.Instance.PlayRandomSFX(beamChargingSFX);
    }

    void AE_LaunchBeam()
    {
        AudioManager.Instance.PlayRandomSFX(beamLaunchSFX);
    }

    void AE_StopBeam()
    {
        StopCoroutine(nameof(ChasingPlayerCoroutine));
        
        StartCoroutine(nameof(BeamCooldownCoroutine));

        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    void LoadProjectiles()
    {
        magazine.Clear();

        if (Physics2D.OverlapBox(playerDetectionTransform.position,playerDetectionSize,0f,playerLayer))
        {
            magazine.Add(projectiles[0]);

            launchSFX = projectileLaunchSFX[0];
        }
        else
        {
            if (Random.value < 0.5f)
            {
                magazine.Add(projectiles[1]);
                launchSFX = projectileLaunchSFX[1];
            }
            else
            {
                for(int i = 2; i < projectiles.Length; i++)
                {
                    magazine.Add(projectiles[i]);
                }

                launchSFX = projectileLaunchSFX[2];
            }
        }
    }

    protected override IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.Instance.BossSpawnPosition(paddingX, paddingY);
        targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            // 假如敵人未抵達該絕對座標
            if (Vector3.Distance(transform.position, targetPosition) >= maxMoveDistancePerFrame)
            {
                // 則繼續往該絕對座標移動
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxMoveDistancePerFrame);
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }

            // 否則
            else
            {
                // 設定新的絕對座標前往
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    protected override IEnumerator RandomlyFireCoroutine()
    {
        
        
        while (isActiveAndEnabled)
        {
            if (GameManager.GameState == GameState.GameOver) yield break;

            if (isBeamReady)
            {
                ActivateBeamWeapon();

                StartCoroutine(nameof(ChasingPlayerCoroutine));

                yield break;
            }
            yield return waitForFireInterval;
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));
        }
    }

    IEnumerator ContinuousFireCoroutine()
    {
        LoadProjectiles();
        
        float continuousFireTimer = 0f;

        while ( continuousFireTimer < continuousFireDuration)
        {
            foreach(var projectile in magazine)
            {
                PoolManager.Release(projectile, muzzle.position);
            }

            continuousFireTimer += minFireInterval;

            AudioManager.Instance.PlayRandomSFX(launchSFX);

            yield return waitForCoutinuousFireInterval;
        }
    }

    IEnumerator BeamCooldownCoroutine()
    {
        yield return waitBeamCooldownTime;

        isBeamReady = true;
    }

    IEnumerator ChasingPlayerCoroutine()
    {
        while (isActiveAndEnabled)
        {

            
            // targetPosition.x = playerTransform.position.x;

            // Viewport.Instance.MaxX - paddingX;

            targetPosition.y = playerTransform.position.y + Viewport.Instance.MaxY - paddingY;

            yield return null;
        }
    }

}
