using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] public class Pool 
{
    public GameObject Prefab
    {
        get
        {
            return prefab;
        }
    }

    public int Size => size;

    public int RuntimeSize => queue.Count;

    [SerializeField] GameObject prefab;
    [SerializeField] int size = 1;

    Queue<GameObject> queue;

    Transform parent;

    //重製對象(初始化函數)
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;

        for(var i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }
    //複製體
    GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab,parent);

        copy.SetActive(false);

        return copy;
    }
    //可調用的對象
    GameObject AvailableObject()
    {
        GameObject availableObject = null;

        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            availableObject = queue.Dequeue();
        }
        else
        {
            availableObject = Copy();
        }

        queue.Enqueue(availableObject);

        return availableObject;
    }
    //已備好的公有對象
    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        return preparedObject;
    }
    //三維向量重載(對象出現在特定位置)
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;

        return preparedObject;
    }
    //四元旋轉參數(對象旋轉並出現在特定位置)
    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }

    //三圍向量縮放(對象縮放並出現在特定位置)
    public GameObject PreparedObject(Vector3 position,Quaternion rotation,Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;

        return preparedObject;
    }
}
