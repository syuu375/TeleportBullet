using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    private enum Follow
    {
        Free,
        BackOfThePlayerHead,   // Behind of player perspective.
    }

    [Header("Component")]
    [SerializeField] private Transform m_target;
    private Camera m_camera;

    [Header("Setting")]
    [SerializeField] private Vector3 m_targetOffset;
    [SerializeField] private Vector2 m_targetPositionOnTheViewportPoint; // y can be 0.
    [SerializeField] private float m_sensitivity;
    [SerializeField] private bool m_isFollowTarget;
    [SerializeField] private Follow m_follow;

    [Header("Movement")]
    [SerializeField] private float m_cameraDistance;    // Distance from the viewport to the camera.
    [SerializeField] private float m_cameraHeight;      // Default camera height.
    //[SerializeField] private float m_approachSpeed;
    //[SerializeField] private float m_leaveSpeed;

    //[Header("State")]
    private Vector3 m_cameraBaseLocalPosition;   // Camera position before shifting.
    private Vector3 m_behindOfTargetXZ;    // Behind of target position, exclude y-coordinate.
    private Vector2 m_currentMousePosition;
    private Vector2 m_pastMousePosition;
    private float m_fAngleX;
    private float m_fAngleY;
    private float m_baseDistance;
    private float m_divBaseDistance;
    private Vector3 m_adjustmentVector;

    // Cache
    private Vector3 m_targetPosition;
    private Vector3 m_cameraPosition;
    private Quaternion m_initialRotation;
    Mouse m_mouseCurrent;
    


    private void Awake()
    {
        Application.targetFrameRate = 60;;
        Initialize();
    }

    private void Update()
    {
        //Vector3 cameraPosition = m_target.right * m_cameraOffset.x + m_target.up * m_cameraOffset.y + m_target.forward * m_cameraOffset.z;

        //m_transform.position = m_target.position + cameraPosition;

        if (!m_isFollowTarget) return;

        UpdateCache();
        UpdateCameraPosition();
        RotationCamera();


        //transform.LookAt(m_lookPosition);
        //transform.LookAt(m_targetPosition);
    }

    private void UpdateCache()
    {
        m_targetPosition = m_target.position + m_targetOffset;
    }

    private void Initialize()
    {
        m_mouseCurrent = Mouse.current;
        m_isFollowTarget = true;

        UpdateBehindTheTarget();
        m_currentMousePosition = m_mouseCurrent.position.ReadValue();
        m_pastMousePosition = m_currentMousePosition;
        m_fAngleX = 0;
        m_fAngleY = 0;
        
        m_initialRotation = Quaternion.identity;
        m_camera = this.GetComponent<Camera>();
        CalculateBaseDistance();

        var tPOTVP = m_targetPositionOnTheViewportPoint;
        if (tPOTVP.x < 0 || 1 < tPOTVP.x)
        {
            Debug.LogWarning($"These are viewport coordinates, so please input values in the range of 0 to 1. m_targetPositionOnTheViewportPoint.x: {tPOTVP.x}");
        }
        if (tPOTVP.y < 0 || 1 < tPOTVP.y)
        {
            Debug.LogWarning($"These are viewport coordinates, so please input values in the range of 0 to 1. m_targetPositionOnTheViewportPoint.y: {tPOTVP.y}");
        }
    }


    private void UpdateCameraPosition()
    {
        //Vector3 horizontalVector = m_targetPosition - m_cameraPosition;
        //horizontalVector.y = 0;
        //float distance = horizontalVector.magnitude;

        switch (m_follow)
        {
            case Follow.Free: m_isFollowTarget = false; break;
            case Follow.BackOfThePlayerHead: UpdateBackTheTargetHead(); break;
        }

        m_cameraPosition = m_targetPosition + m_cameraBaseLocalPosition;
        transform.position = m_cameraPosition;

        AdjustCameraPosition();

        transform.position = m_cameraPosition + m_adjustmentVector;
    }


    // Camera position before adjustment.
    private void UpdateBaseLocalPosition(Vector3 position)
    {
        m_cameraBaseLocalPosition = position;
        m_cameraBaseLocalPosition.y += m_cameraHeight;
    }

    // Get behind of target horizontal position.
    private void UpdateBehindTheTarget()
    {
        Vector3 behind = -m_target.forward;
        Vector2 horizontalVector = new Vector2(behind.x, behind.z).normalized;
        m_behindOfTargetXZ = new Vector3(horizontalVector.x, 0, horizontalVector.y); 

        float distanceXZ = Mathf.Sqrt(m_cameraDistance * m_cameraDistance - m_cameraHeight * m_cameraHeight);
        Vector3 localPosition = m_behindOfTargetXZ * distanceXZ;

        UpdateBaseLocalPosition(localPosition);
    }

    // Behind of target direction.
    private void UpdateBackTheTargetHead()
    {
        Vector3 behindOfLocalPos = (-transform.forward) * m_cameraDistance;
        UpdateBaseLocalPosition(m_targetPosition + behindOfLocalPos);
    }


    private void RotationCamera()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        m_currentMousePosition = m_mouseCurrent.position.ReadValue();


        m_fAngleX += (m_currentMousePosition.x - m_pastMousePosition.x) * m_sensitivity;
        m_fAngleY += (m_currentMousePosition.y - m_pastMousePosition.y) * m_sensitivity;

        if (m_fAngleX > 360.0f) m_fAngleX -= 360.0f;
        if (m_fAngleY < -90.0f) m_fAngleY = -90.0f;
        else if (m_fAngleY > 90.0f) m_fAngleY = 90.0f;

        transform.rotation = m_initialRotation;
        transform.Rotate(-m_fAngleY, m_fAngleX, 0.0f, Space.World);
        //transform.Rotate(-m_fAngleY * 360.0f / (2.0f * Mathf.PI), m_fAngleX * 360.0f / (2.0f * Mathf.PI), 0.0f, Space.World);

        m_pastMousePosition = m_currentMousePosition;
    }

    private void AdjustCameraPosition()
    {
        Vector2 currentTargetPosition = m_camera.WorldToViewportPoint(m_targetPosition);
        Vector3 difference = currentTargetPosition - m_targetPositionOnTheViewportPoint;

        m_adjustmentVector = (transform.right * difference.x * m_divBaseDistance);// - (transform.up * difference.y * m_divBaseDistance);
    }


    public void SetIsFollow(bool value)
    {
        m_isFollowTarget = value;
    }

    // カメラから1m前方のオブジェクトが1m横に移動した際のスクリーン上の移動距離
    // The distance an object - located 1m in front of the camera - moves on the screen when it shifts 1m laterally.
    private void CalculateBaseDistance()
    {
        Vector3 Temporary0 = transform.forward;
        Vector3 Temporary1 = Temporary0 + transform.right;

        //Vector2 v2Pos0 = m_camera.WorldToScreenPoint(Temporary0);
        //Vector2 v2Pos1 = m_camera.WorldToScreenPoint(Temporary1);

        Vector2 v2Pos0 = m_camera.WorldToViewportPoint(Temporary0);
        Vector2 v2Pos1 = m_camera.WorldToViewportPoint(Temporary1);

        m_baseDistance = Vector2.Distance(v2Pos0, v2Pos1);

        // m_baseDistance won't become 0.
        m_divBaseDistance = (1 / m_baseDistance);// * m_cameraDistance;
    }
}
