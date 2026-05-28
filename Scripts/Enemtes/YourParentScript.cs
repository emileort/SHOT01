using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YourParentScript : MonoBehaviour
{
   
    public Transform[] childTransforms;
    private Vector3[] originalChildPositions;
    private Quaternion[] originalChildRotations;
    private PolygonCollider2D parentCollider;

    void Start()
    {
        // 获取所有子物体
        
        originalChildPositions = new Vector3[childTransforms.Length];
        originalChildRotations = new Quaternion[childTransforms.Length];

        // 保存每个子物体的原始位置和旋转信息
        for (int i = 0; i < childTransforms.Length; i++)
        {
            originalChildPositions[i] = childTransforms[i].position;
            originalChildRotations[i] = childTransforms[i].rotation;
        }

        // 获取父物体的 PolygonCollider2D 组件
        parentCollider = GetComponent<PolygonCollider2D>();
    }

    void LateUpdate()
    {
        // 更新 PolygonCollider2D 的位置
        UpdateColliderPosition();
    }

    void UpdateColliderPosition()
    {
        // 遍历所有子物体
        for (int i = 0; i < childTransforms.Length; i++)
        {
            // 获取子物体的 PolygonCollider2D 组件
            PolygonCollider2D childCollider = childTransforms[i].GetComponent<PolygonCollider2D>();

            if (childCollider != null)
            {
                // 将子物体的碰撞体合并到父物体的碰撞体
                if (parentCollider != null)
                {
                    parentCollider.pathCount += childCollider.pathCount;

                    for (int j = 0; j < childCollider.pathCount; j++)
                    {
                        Vector2[] childPath = childCollider.GetPath(j);

                        // 将子物体的本地坐标转换为世界坐标
                        Vector2[] worldPoints = new Vector2[childPath.Length];
                        for (int k = 0; k < childPath.Length; k++)
                        {
                            worldPoints[k] = childTransforms[i].TransformPoint(childPath[k]);
                        }

                        // 将世界坐标转换为父物体的本地坐标
                        for (int k = 0; k < worldPoints.Length; k++)
                        {
                            worldPoints[k] = transform.InverseTransformPoint(worldPoints[k]);
                        }

                        // 设置父物体碰撞体的路径
                        parentCollider.SetPath(j + parentCollider.pathCount - 1, worldPoints);
                    }

                }
            }
            // 啟動協程，延遲0.1秒刪除舊的碰撞體
            //StartCoroutine(DestroyOldCollider(parentCollider, 1.2f));
        }

        IEnumerator DestroyOldCollider(PolygonCollider2D collider, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(collider);
        }
    }
}


