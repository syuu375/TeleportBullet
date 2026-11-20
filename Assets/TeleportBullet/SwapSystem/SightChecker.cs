using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SightData
{
    [ReadOnly] public Vector3 origin;      // 起点
    [ReadOnly] public Vector3 direction;   // 方向
    public float maxDistance;   // 射程距離
    public float viewingAngle;  // 視野角
}

public class SightChecker
{
    public SightChecker(bool isVisible)
    {
        m_isVisible = isVisible;
    }

    private bool m_isVisible;   // Display gizmos in the Unity Editor.
    private RayCollider m_rayCollider = new(true);
    private List<Vector3> m_objectsPos;

    /// <summary>
    /// 設定された視野角内、視点の中心に最も近いオブジェクト、又はRayに当たったオブジェクトに照準を合わせます。
    /// </summary>
    public GameObject AutoAim(SightData sightData)
    {
        GameObject obj = new();
        RayData rayData = new()
        {
            origin = sightData.origin,
            direction = sightData.direction,
            maxDistance = sightData.maxDistance
        };
        obj = m_rayCollider.EmitCollider(rayData).gameObject;
        if (obj != null) return obj;

        for (int i = 0; i < m_objectsPos.Count; i++)
        {

        }
        return obj;
    }

    // Is the target within the field of view.
    private bool IsVisible(SightData sightData, Vector3 targetPosition)
    {
        Vector3 selfPos = sightData.origin;
        Vector3 selfDir = sightData.direction;

        // Calculation of direction and distance to the target.
        Vector3 targetDir = targetPosition - selfPos;
        float targetDistance = targetDir.magnitude;

        float cosHalf = Mathf.Cos(sightData.viewingAngle * 0.5f * Mathf.Deg2Rad);
        float innerProduct = Vector3.Dot(selfDir, targetDir.normalized);

        return innerProduct > cosHalf && targetDistance < sightData.maxDistance;
    }

    public void UpdateObjTransform(Transform selfTransform, List<Vector3> objectsPos)
    {
        m_objectsPos = new();
        for (int i = 0; i < objectsPos.Count; i++)
        {
            if (selfTransform.InverseTransformPoint(objectsPos[i]).z < 0) continue; // オブジェクトの位置が後方なら無視
            m_objectsPos.Add(objectsPos[i]);
        }
    }
}
