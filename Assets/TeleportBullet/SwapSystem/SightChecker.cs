using UnityEngine;

[System.Serializable]
public class SightData
{
    public Vector3 origin;      // 起点
    public Vector3 direction;   // 方向
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

    /// <summary>
    /// 設定された視野角内、視点の中心に最も近いオブジェクト、又はRayに当たったオブジェクトに照準を合わせます。
    /// </summary>
    public GameObject AutoAim(SightData sightData)
    {
        GameObject obj = new();
        
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
}
