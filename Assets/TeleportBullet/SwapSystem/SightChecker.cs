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
    private List<Transform> m_objects = new();

    /// <summary>
    ///（オブジェクトの中心が）設定された視野角内 且つ 視点の中心に最も近いオブジェクト、又はRayに当たったオブジェクトに照準を合わせます。※ Rayが優先
    /// </summary>
    public Transform AutoAim(SightData sightData)
    {
        Transform obj;
        RayData rayData = new()
        {
            origin = sightData.origin,
            direction = sightData.direction,
            maxDistance = sightData.maxDistance
        };

        if (m_isVisible)
        {
            //float distanceZ = Mathf.Cos(sightData.viewingAngle);
            float distanceXY = Mathf.Sin(sightData.viewingAngle);
            Vector3 angleRight  = new Vector3( distanceXY, 0, 0);
            Vector3 angleLeft   = new Vector3(-distanceXY, 0, 0);
            Vector3 angleUp     = new Vector3(0,  distanceXY, 0);
            Vector3 angleDown   = new Vector3(0, -distanceXY, 0);

            Debug.DrawRay(sightData.origin, (sightData.direction + angleRight).normalized * sightData.maxDistance, Color.red, 2.0f); // Right
            Debug.DrawRay(sightData.origin, (sightData.direction + angleLeft ).normalized * sightData.maxDistance, Color.red, 2.0f); // Left
            Debug.DrawRay(sightData.origin, (sightData.direction + angleUp   ).normalized * sightData.maxDistance, Color.red, 2.0f); // Up
            Debug.DrawRay(sightData.origin, (sightData.direction + angleDown ).normalized * sightData.maxDistance, Color.red, 2.0f); // Down
        }

        obj = m_rayCollider.EmitCollider(rayData)?.gameObject.transform;
        if (obj != null) return obj;

        float nearDistance = sightData.maxDistance;
        //Debug.Log($"origin: {sightData.origin}, direction: {sightData.direction}, maxDistance: {sightData.maxDistance}, viewingAngle: {sightData.viewingAngle}");
        for (int i = 0; i < m_objects.Count; i++)
        {
            if (!IsVisible(sightData, m_objects[i].position)) continue;

            float distance = (m_objects[i].position - sightData.origin).magnitude;
            Debug.Log($"[AA]i: {i}, magnitude: {distance}");

            if (nearDistance <= distance) continue;
            nearDistance = distance;
            obj = m_objects[i];
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

    public void UpdateObjTransform(Transform selfTransform, List<Transform> objects)
    {
        m_objects.Clear();
        for (int i = 0; i < objects.Count; i++)
        {
            if (!objects[i].gameObject.activeSelf) continue;    // 非アクティブのオブジェクトは無視
            if (selfTransform.InverseTransformPoint(objects[i].position).z < 0) continue; // オブジェクトの位置が後方なら無視
            m_objects.Add(objects[i]);
        }
    }
}
