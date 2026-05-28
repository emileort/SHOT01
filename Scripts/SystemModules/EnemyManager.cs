using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];

    public int WaveNumber => waveNumber;

    public float TimeBetweenWave => timeBetweenWaves;
    
    [SerializeField] bool spawnEnemy = true;

    [SerializeField] GameObject waveUI;
    
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float timeBetweenSpawns = 1f;

    [SerializeField] float timeBetweenWaves = 1f;

    [SerializeField] int minEnemyAmount = 4;
    [SerializeField] int maxEnemyAmount = 10;
    // 指定要生成的敵人種類和數量
    

    [SerializeField] GameObject bossPrefab;
    [SerializeField] int bossWaveNumber;

    int waveNumber = 1;
    [SerializeField] int numberOfEnemyType1 = 3;  // 假設要生成 5 個敵人種類 1
    [SerializeField] int numberOfEnemyType2 = 4;  // 假設要生成 3 個敵人種類 2
    [SerializeField] int numberOfEnemyType3 = 10;  // 假設要生成 3 個敵人種類 2
    int enemyAmount;

    [SerializeField] GameObject EnemyType1;

    [SerializeField] GameObject EnemyType2;

    [SerializeField] GameObject EnemyType3;

    List<GameObject> enemyList;

    WaitForSeconds waitTimeBetweenSpawns;

    WaitForSeconds waitTimeBetweenWaves;

    WaitUntil waitUntilNoEnemy;

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        // waitUntilNoEnemy = new WaitUntil(NoEnemy);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);
    }

    // bool NoEnemy()
    // {
    //    return enemyList.Count == 0;
    // }

    IEnumerator Start()
    {
        while (spawnEnemy && GameManager.GameState!=GameState.GameOver)
        {
            
            waveUI.SetActive(true);

            yield return waitTimeBetweenWaves;

            waveUI.SetActive(false);

            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
        
    }

    IEnumerator RandomlySpawnCoroutine()
    {
        if (waveNumber % bossWaveNumber==0)
        {
            //生成BOSS
            var boss = PoolManager.Release(bossPrefab);

            enemyList.Add(boss);
        }
        else
        {
            enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWaveNumber, maxEnemyAmount);

            

            for (int i = 0; i < numberOfEnemyType1; i++)
            {
                var enemyType1 = PoolManager.Release(EnemyType1);
                enemyList.Add(enemyType1);
                yield return waitTimeBetweenSpawns;
            }

            for (int i = 0; i < numberOfEnemyType2; i++)
            {
                var enemyType2 = PoolManager.Release(EnemyType2);
                enemyList.Add(enemyType2);
                yield return waitTimeBetweenSpawns;
            }
            for (int i = 0; i < numberOfEnemyType3; i++)
            {
                var enemyType3 = PoolManager.Release(EnemyType3);
                enemyList.Add(enemyType3);
                yield return waitTimeBetweenSpawns;
            }

            /*
            for (int i = 0; i < enemyAmount; i++)
            {
                // var enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                // PoolManager.Release(enemy);
                enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));

                yield return waitTimeBetweenSpawns;
            }
            */
        }
    
        

        yield return waitUntilNoEnemy;

        waveNumber++;
    }

    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);
}
