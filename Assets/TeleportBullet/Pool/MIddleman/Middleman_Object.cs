using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middleman_Object : MiddlemanBase<Enum_EnemyType, ComponentPoolHandler_Object, Test_EnemyController>
{
    [SerializeField, ReadOnly] private List<Transform> m_objects = new();
    public List<Transform> Objects => m_objects;

    private void Awake()
    {
        //Debug.Log("[MO]awake");
        StartCoroutine(StoreObjects());
    }

    private IEnumerator StoreObjects()
    {
        yield return new WaitForEndOfFrame();

        //Debug.Log("[MO.SO]");
        //（オート照準の対象となる）すべての "Object" を格納。（effect等を除く）
        foreach (var objectPool in pools)
        {
            List<GameObject> objects = objectPool.pool.Objects;
            foreach (var obj in objects)
            {
                m_objects.Add(obj.transform);
            }
        }
    }
}
