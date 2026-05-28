using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("----MOVE----")]

    [SerializeField] protected float paddingX;
    [SerializeField] protected float paddingY;
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float moveRotationAngle = 25f;

    [Header("----FIRE----")]

    [SerializeField] protected GameObject[] projectiles;

    [SerializeField] protected AudioData[] projectileLaunchSFX;

    [SerializeField] protected Transform muzzle;

    [SerializeField] protected float minFireInterval;

    [SerializeField] protected float maxFireInterval;

    protected Vector3 targetPosition;

    protected float maxMoveDistancePerFrame;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    protected virtual void Awake()
    {
        maxMoveDistancePerFrame = moveSpeed * Time.fixedDeltaTime;

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x ;
        paddingY = size.y ;
    }


    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    protected virtual IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);
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
    protected virtual IEnumerator RandomlyFireCoroutine()
    {
        //WaitForSeconds waitForRandomFireInterval= new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

        while (gameObject.activeSelf)
        {
            //yield return waitForRandomFireInterval;
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            if (GameManager.GameState == GameState.GameOver) yield break;

            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }

            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
        }
    }
}
