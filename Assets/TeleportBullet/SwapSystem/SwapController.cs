using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

//Å•âº----------------------
public class  EntityData
{
    private int id;

    public int ID => id;
}
//Å£------------------------


public class SwapController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private VisualEffect m_visualEffectPrefab;
    [SerializeField] private Camera m_camera;

    [Header("Setting")]
    [SerializeField] private float m_coolTime;      //Time until teleportation becomes available again
    [SerializeField] private float m_preparatoty;   // Swap wind-up Time
    [SerializeField] private float m_afterSwap;     // Time after swap

    private RayCollider m_rayCollider = new(true);

    public void EmitRay(Transform myTransform)
    {
        RayData rayData = new()
        {
            origin = myTransform.position,
            direction = myTransform.forward
        };
        Collider target = m_rayCollider.EmitCollider(rayData);

        if (target == null) return;
        StartCoroutine(LightEmissionOfSwap());
    }

    private void SetAiming()
    {

    }

    private IEnumerator LightEmissionOfSwap()
    {
        yield return PlayAnimation();
    }

    private IEnumerator PlayAnimation()
    {
        yield return null;
    }
}
