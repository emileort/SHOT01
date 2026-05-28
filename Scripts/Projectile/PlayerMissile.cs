using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{

    [SerializeField] AudioData targetAcquiredVoice = null;

    [Header("===== SPEED CHANGE =====")]

    [SerializeField] float lowSpeed = 8f;
    [SerializeField] float highSpeed = 25f;
    [SerializeField] float variableSpeefDelay = 0.5f;

    [Header("===== EXPLOSION =====")]

    [SerializeField] GameObject explosionVFX = null;

    [SerializeField] AudioData explosionSFX = null;
    
    [SerializeField] LayerMask enemyLayerMask = default;

    [SerializeField] float explosionRadius = 3f;

    [SerializeField] float explosionDamage = 100f;

    WaitForSeconds waitVariableSpeefDelay;

    protected override void Awake()
    {
        base.Awake();
        waitVariableSpeefDelay = new WaitForSeconds(variableSpeefDelay);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VarlableSpeedCoroutione));
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        // 爆炸特效
        PoolManager.Release(explosionVFX, transform.position);
        // 爆炸音效
        AudioManager.Instance.PlayRandomSFX(explosionSFX);
        // 對範圍內敵人造成傷害
        var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayerMask);

        foreach(var collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(explosionDamage);
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }


    IEnumerator VarlableSpeedCoroutione()
    {
        moveSpeed = lowSpeed;

        yield return waitVariableSpeefDelay;

        moveSpeed = highSpeed;

        if (target != null)
        {
            AudioManager.Instance.PlayRandomSFX(targetAcquiredVoice);
        }
    }
}
