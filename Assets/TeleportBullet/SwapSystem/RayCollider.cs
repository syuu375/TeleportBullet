using UnityEngine;

[System.Serializable]
public class RayData
{
    public Vector3 origin;      // ‹N“_
    public Vector3 direction;   // •ûŒü
    public float range;         // ŽË’ö‹——£
}

public class RayCollider
{
    public RayCollider(bool isVisible)
    {
        m_isVisible = isVisible;
    }

    private Ray m_ray;
    private bool m_isVisible;   // Display ray collision detection in the Unity Editor.


    public Collider EmitCollider(RayData rayData)
    {
        Debug.Log("EmitCollider");
        m_ray = new Ray(rayData.origin, rayData.direction);

        if (m_isVisible)
        {
            Debug.DrawRay(rayData.origin, rayData.direction * rayData.range, Color.red, 2.0f);
        }

        RaycastHit hit;
        Physics.Raycast(m_ray, out hit, rayData.range);

        return hit.collider;
    }
}
