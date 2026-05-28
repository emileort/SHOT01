using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZFormationEnemyController : EnemyController
{
    [Header("----Z Formation Settings----")]

    [SerializeField] int groupSize = 5;  // 每組敵人的數量
    [SerializeField] float formationDistance = 1.5f;  // 敵人之間的距離

    [SerializeField] float minAmgle = 50f;

    [SerializeField] float maxAmgle = 75f;

    Enemy enemy;

    float Angle;

    bool isMovingDown = true;  // 新增變數，表示是否在往下移動
    bool isMovingRight = true; // 新增變數，表示是否持續往右移動



    protected override void Awake()
    {
        enemy = GetComponent<Enemy>();
        base.Awake();
    }

    protected override void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));

        // 啟動計時器，10秒後執行自動消失的邏輯
        StartCoroutine(AutoDisappearCoroutine());
    }

    protected override IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.Instance.ZEnemySpawnPosition(paddingX, paddingY);
        Angle = Random.Range(minAmgle, maxAmgle);

        while (gameObject.activeSelf)
        {
            Vector3 targetPosition;

            if (isMovingDown)
            {
                if (isMovingRight)
                {
                    targetPosition = Viewport.Instance.ZEnemySpawnPosition(0, 0);
                }
                else
                {
                    targetPosition = Viewport.Instance.ZEnemySpawnPosition(-10, 0);
                    
                }

            }
            else
            {
                if (isMovingRight)
                {
                    targetPosition = Viewport.Instance.ZEnemySpawnPosition(-5, 10);
                }
                else
                {
                    targetPosition = Viewport.Instance.ZEnemySpawnPosition(-15, 10);
                    
                }


            }

            if (Vector3.Distance(transform.position, targetPosition) >= maxMoveDistancePerFrame)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxMoveDistancePerFrame);
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
                // transform.rotation *= Quaternion.Euler(0f, 0f, Angle);
            }
            else
            {
                // 轉換移動方向
                isMovingDown = !isMovingDown;
                if (isMovingDown)
                {
                    isMovingRight = !isMovingRight;
                }

            }

            yield return new WaitForFixedUpdate();
        }
    }

    // 新增的 Coroutine，用於計時並自動消失
    protected IEnumerator AutoDisappearCoroutine()
    {
        yield return new WaitForSeconds(8f);

        // 自動刪除
        enemy.Die();
    }


}
