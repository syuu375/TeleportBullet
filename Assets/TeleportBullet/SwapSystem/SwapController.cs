using UnityEngine;
using System.Collections;
using UnityEngine.VFX;
using UnityEngine.InputSystem;

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
    [SerializeField] private Transform m_marker;    // Target marker
    [SerializeField] private Animator m_markerAnim; // Marker animation

    [Header("Parameter")]
    [SerializeField] private float m_coolTime;      // Time until teleportation becomes available again
    [SerializeField] private float m_preparatory;   // Swap wind-up Time
    [SerializeField] private float m_afterSwap;     // Time after swap
    [SerializeField] private float m_rangeOfRay;

    private PlayerInputActions m_inputAction;

    private RayCollider m_rayCollider = new(true);
    private Transform m_transform;  // my transform
    private Transform m_target;     // target transform
    private bool m_hasTargetChanged;    // Becomes true only when the target is changed.

    private void Awake()
    {
        Application.targetFrameRate = 60;
        m_transform = transform;

        m_inputAction = new PlayerInputActions();
        m_inputAction.Player.AttackTeleport.performed += OnAttackTeleport;

        m_inputAction.Enable();
    }

    private void Update()
    {
        EmitSkillSwap(m_transform);
    }

    public void EmitSkillSwap(Transform myTransform)
    {
        RayData rayData = new()
        {
            origin = myTransform.position,
            direction = myTransform.forward,
            range = m_rangeOfRay
        };
        Collider target = m_rayCollider.EmitCollider(rayData);

        if (target == null)
        {
            Debug.Log("[ESS]target == null");
            m_target = null;
            m_marker.gameObject.SetActive(false);
            return;
        }

        Debug.Log($"[ESS]target: {target.gameObject}");
        Transform targetTransform = target.gameObject.transform;
        if (m_target == targetTransform) m_hasTargetChanged = true;
        m_target = targetTransform;
        SetAiming(m_target);
    }


    private void SetAiming(Transform target)
    {
        if (m_hasTargetChanged)
        {
            m_marker.gameObject.SetActive(true);
            m_markerAnim.SetTrigger("LockOn");
        }


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
        //vfx animation(warp effect)
        //yield return null;
    }

    public void Swap(ref Vector3 myself, ref Vector3 target)
    {
        Vector3 myPosition = myself;
        myself = target;
        target = myPosition;
    }

    private void OnAttackTeleport(InputAction.CallbackContext context)
    {
        Debug.Log("OnAttackTeleport");

        if (m_target == null)
        {
            Debug.Log("[OAT]target == null");
            return;
        }
        Debug.Log($"m_target: {m_target.position}");
    }
}
