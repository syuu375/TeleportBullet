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

    [Header("Parameter")]
    [SerializeField] private float m_coolTime;      //Time until teleportation becomes available again
    [SerializeField] private float m_preparatory;   // Swap wind-up Time
    [SerializeField] private float m_afterSwap;     // Time after swap

    private RayCollider m_rayCollider = new(true);
    private Transform m_transform;  // my transform
    private Transform m_target;     // target transform

    private void Awake()
    {
        Application.targetFrameRate = 60;
        m_transform = transform;
    }

    public void EmitSkillSwap(Transform myTransform)
    {
        RayData rayData = new()
        {
            origin = myTransform.position,
            direction = myTransform.forward
        };
        Collider target = m_rayCollider.EmitCollider(rayData);

        if (target == null) return;
        m_target = target.gameObject.transform;
        SetAiming(m_target);
    }

    private void Start()
    {
        StartCoroutine(LightEmissionOfSwap());
    }


    private void SetAiming(Transform target)
    {

    }

    private IEnumerator LightEmissionOfSwap()
    {
        PlayAnimation();
        float time = 0;

        while(time < m_preparatory)
        {
            time += Time.deltaTime;
            yield return null;
        }

        Vector3 myPosition = m_transform.position;
        Vector3 tarPosition = m_target.position;
        Swap(ref myPosition, ref tarPosition);

        m_transform.position = myPosition;
        m_target.position = tarPosition;
    }

    private void PlayAnimation()
    {
        //yield return null;
    }

    public void Swap(ref Vector3 myself, ref Vector3 target)
    {
        Vector3 myPosition = myself;
        myself = target;
        target = myPosition;
    }
}
