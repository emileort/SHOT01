using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//如果我想改成敵人出現的十秒鐘後直接死亡，期間敵人移動照常移動與運行，我該怎麼做
public class SFormationEnemyController : EnemyController
{
    [Header("----S Formation Settings----")]

    [SerializeField] int groupSize = 5;  // 每組敵人的數量
    [SerializeField] float formationDistance = 1.5f;  // 敵人之間的距離

    [SerializeField] float minAmgle = 50f;

    [SerializeField] float maxAmgle = 75f;

    Enemy enemy;

    float Angle;

    int currentGroupIndex = 0;

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
        transform.position = Viewport.Instance.EnemySpawnPosition(paddingX, paddingY);
        Angle = Random.Range(minAmgle, maxAmgle);

        while (gameObject.activeSelf)
        {
            Vector3 targetPosition;

            if (isMovingDown)
            {
                if (isMovingRight)
                {
                    targetPosition = Viewport.Instance.EnemySpawnPosition(-5, -10);
                }
                else
                {
                    targetPosition = Viewport.Instance.EnemySpawnPosition(-15, -10);
                }
                
            }
            else
            {
                if (isMovingRight)
                {
                    targetPosition = Viewport.Instance.EnemySpawnPosition(-10, 0);
                }
                else
                {
                    targetPosition = Viewport.Instance.EnemySpawnPosition(-20, 0);
                    
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

    Vector3 GetNextSFormationPosition()
    {
        int row = currentGroupIndex / groupSize;  // 行
        int col = currentGroupIndex % groupSize;  // 列

        float xOffset = col * formationDistance;
        float yOffset = Mathf.Pow(-1, row) * row * formationDistance;  // 使用 pow 函數實現 S 型縱隊

        Vector3 nextPosition = new Vector3(xOffset * 5, yOffset * 5, 0f) + new Vector3(xOffset * 5, yOffset * 5, 0f);

        // 準備下一個位置
        currentGroupIndex++;
        if (currentGroupIndex >= groupSize)
        {
            currentGroupIndex = 0;
        }

        return nextPosition;
    }


    Vector3 SFormationPosition()
    {
        int row = currentGroupIndex / groupSize;  // 行
        int col = currentGroupIndex % groupSize;  // 列

        float xOffset = col * formationDistance;
        float yOffset = Mathf.Pow(-1, row) * row * formationDistance;  // 使用 pow 函數實現 S 型縱隊

        Vector3 nextPosition = Viewport.Instance.EnemySpawnPosition(paddingX, paddingY) + new Vector3(xOffset, yOffset, 0f) ;

        // 準備下一個位置
        currentGroupIndex++;
        if (currentGroupIndex >= groupSize)
        {
            currentGroupIndex = 0;
        }

        return nextPosition;
    }

    // 新增的 Coroutine，用於計時並自動消失
    protected IEnumerator AutoDisappearCoroutine()
    {
        yield return new WaitForSeconds(8f);

        // 自動刪除
        enemy.Die();
    }
}
