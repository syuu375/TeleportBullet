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
    [SerializeField] private MyAnimator m_markerAnim;

    [Header("Parameter")]
    [SerializeField] private SightData m_sightData;
    [SerializeField] private float m_coolTime;      // Time until teleportation becomes available again
    [SerializeField] private float m_preparatory;   // Swap wind-up Time
    [SerializeField] private float m_afterSwap;     // Time after swap
    [SerializeField] private float m_rangeOfRay;

    private PlayerInputActions m_inputAction;

    private SightChecker m_sightChecker = new(true);
    private RayCollider m_rayCollider = new(true);
    private Transform m_transform;      // self transform
    private bool m_hasTargetChanged;    // Becomes true only when the target is changed.

    [Header("Current Target")]
    [SerializeField, ReadOnly] private Transform m_target;     // target transform

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
        UpdateSightData();
        EmitSkillSwap(m_transform);
    }

    private void UpdateSightData()
    {
        m_sightData.origin = m_transform.position;
        m_sightData.direction = m_transform.forward;
    }

    public void EmitSkillSwap(Transform myTransform)
    {
        RayData rayData = new()
        {
            origin = myTransform.position,
            direction = myTransform.forward,
            maxDistance = m_rangeOfRay
        };
        Collider target = m_rayCollider.EmitCollider(rayData);

        if (target == null)
        {
            //Debug.Log("[ESS]target == null");
            m_target = null;
            m_marker.gameObject.SetActive(false);
            return;
        }

        //Debug.Log($"[ESS]target: {target.gameObject}");
        Transform targetTransform = target.gameObject.transform;
        if (m_target == targetTransform) m_hasTargetChanged = false;
        else m_hasTargetChanged = true;

        m_target = targetTransform;
        SetAiming(m_target);
    }

    private void SetAiming(Transform target)
    {
        if (m_hasTargetChanged)
        {
            if (!m_marker.gameObject.activeSelf)
            {
                m_marker.gameObject.SetActive(true);
            }
            //m_markerAnim.SetTrigger("LockOn");
            m_markerAnim.SetTrigger(0); // lock on animation
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

    // Play animation of swap.
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
        //Debug.Log("OnAttackTeleport");

        if (m_target == null)
        {
            //Debug.Log("[OAT]target == null");
            return;
        }
        Debug.Log($"[OAT]m_target: {m_target.position}");
        StartCoroutine(LightEmissionOfSwap());
    }
}
