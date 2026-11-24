using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ComponentPoolHandler<T> : ObjectPoolManager where T : Component
{
    protected List<T> m_objectComponent;

    [Header("Å• ÉIÅ[Égè∆èÄÇÃëŒè€Ç…ä‹ÇﬂÇÈÇ© Å•")]
    [SerializeField] private bool m_isObject;   // is ComponentPoolHander_Object
    private List<GameObject> m_objects = new();
    public List<GameObject> Objects => m_objects;

    public override void Initialize()
    {
        m_objectComponent = new();
        base.Initialize();
    }

    public T GetComponentFromPool()
    {
        return GetFromPool(m_objectComponent);
    }

    protected override int AddToPool()
    {
        int index = base.AddToPool();
        m_objectComponent.Add(m_objectPool.ElementAt(index).GetComponent<T>());
        if (m_isObject)
        {
            //Debug.Log($"[CPH.ATP]isObject == true, index: {index}");
            m_objects.Add(m_objectPool[index]);
        }

        return index;
    }
}
