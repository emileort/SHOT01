using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] Projectile projectile;

    [SerializeField] float minBallisticAmgle = 50f;

    [SerializeField] float maxBallisticAmgle = 75f;

    float ballisticAngle;

    Vector3 targetDirection;

    public IEnumerator HomingCoroutine(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAmgle, maxBallisticAmgle);


        while (gameObject.activeSelf)
        {
            
            if (target.activeSelf)
            {
                // 往目標移動
                targetDirection = target.transform.position - transform.position;
                // 讓子彈朝向目標
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.forward);

                transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle); 
                // 移動子彈
                projectile.Move();
            }
            else
            {
                projectile.Move();
            }

            yield return null;
        }
    }
}
