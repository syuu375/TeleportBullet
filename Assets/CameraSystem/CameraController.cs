using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Vector3 m_lookPosition;

    [Header("Component")]
    [SerializeField] private Transform m_target;

    [Header("Setting")]
    //[SerializeField] private Vector3 m_cameraOffset;
    [SerializeField] private Vector3 m_targetOffset;
    [SerializeField] private Vector2 m_playersPositionOnTheScreen;

    [Header("Rotation")]
    [SerializeField] private float m_lookPlayDistance;
    [SerializeField] private float m_followSpeed;
    [SerializeField] private float m_rotationSpeed;
    //[SerializeField] private float m_turningAcceleration;

    [Header("Movement")]
    [SerializeField] private float m_cameraDistance;    // Distance from the viewport to the camera.
    [SerializeField] private float m_cameraHeight;      // Default camera height.
    [SerializeField] private float m_currentCameraHeight;   // test
    [SerializeField] private float m_cameraPlayDistance;
    [SerializeField] private float m_leaveSpeed;

    [Header("State")]
    [SerializeField] private Vector3 m_cameraBaseLocalPosition;   // Camera position before shifting.
    [SerializeField] private Vector3 m_behindOfTargetXZ;    // Behind of target position, exclude y-coordinate.
    [SerializeField] private Vector2 m_currentMousePosition;
    private Vector2 m_pastMousePosition;
    //[SerializeField] private float m_acceleration;
    //[SerializeField] private float m_velocityConvergence;
    //[SerializeField] private float m_turningVelocityConvergence;

    // Cache
    private Vector3 m_cameraPosition;
    private Vector3 m_targetPosition;
    [SerializeField] private Quaternion m_cameraRotation;
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
        UpdateCache();
        //UpdateLookPosition();
        UpdateCameraPosition();

        RotationCamera();

        //transform.LookAt(m_lookPosition);
        //transform.LookAt(m_targetPosition);
    }

    private void UpdateCache()
    {
        m_cameraPosition = transform.position;
        m_targetPosition = m_target.position + m_targetOffset;
        m_cameraRotation = transform.rotation;
    }

    private void Initialize()
    {
        m_mouseCurrent = Mouse.current;

        UpdateBaseLocalPosition();
        m_currentMousePosition = m_mouseCurrent.position.ReadValue();
        m_pastMousePosition = m_currentMousePosition;
        
    }

    private void UpdateLookPosition()
    {
        Vector3 vector = m_targetPosition - m_cameraPosition - m_lookPosition;
        float distance = vector.magnitude;

        if (distance > m_lookPlayDistance)
        {
            float moveDistance = (distance - m_lookPlayDistance) * (Time.deltaTime * m_followSpeed);
            m_lookPosition += vector.normalized * moveDistance;
        }

        //m_transform.rotation = Quaternion.LookRotation(m_direction);
    }

    private void UpdateCameraPosition()
    {
        Vector3 horizontalVector = m_targetPosition - m_cameraPosition;
        horizontalVector.y = 0;
        float distance = horizontalVector.magnitude;

        float moveDistance = 0;
        if (distance > m_cameraDistance + m_cameraPlayDistance)
        {
            moveDistance = distance - (m_cameraDistance + m_cameraPlayDistance);
            moveDistance *= Time.deltaTime * m_followSpeed;
        }
        else if (distance < m_cameraDistance + m_cameraPlayDistance)
        {
            moveDistance = distance - (m_cameraDistance - m_cameraPlayDistance);
            moveDistance *= Time.deltaTime * m_leaveSpeed;
        }

        //UpdateBasePosition();
        UpdateBehindOfTarget();
        UpdateBaseLocalPosition();

        transform.position = m_targetPosition + m_cameraBaseLocalPosition;
        //Vector3 cameraPosition = m_cameraPosition + m_behindOfTargetXZ * moveDistance;
        //cameraPosition.y = m_lookPosition.y + m_cameraHeight;
        //transform.position = cameraPosition;
        //m_currentCameraBasePosition += (horizontalVector * moveDistance);

        //transform.position = m_currentCameraBasePosition;
    }

    private void UpdateBaseLocalPosition()
    {
        float distanceXZ = Mathf.Sqrt(m_cameraDistance * m_cameraDistance - m_cameraHeight * m_cameraHeight);
        m_cameraBaseLocalPosition = m_behindOfTargetXZ * distanceXZ;
        m_cameraBaseLocalPosition.y = m_cameraHeight;
    }

    // Get behind of target horizontal position.
    private void UpdateBehindOfTarget()
    {
        Vector3 behind = -m_target.forward;
        Vector2 horizontalVector = new Vector2(behind.x, behind.z).normalized;
        m_behindOfTargetXZ = new Vector3(horizontalVector.x, 0, horizontalVector.y);
    }

    // Controlled by player
    private void RotationCamera()
    {
        m_currentMousePosition = m_mouseCurrent.position.ReadValue();
        
        Vector2 moveDistance = m_currentMousePosition - m_pastMousePosition;
        this.transform.rotation = Quaternion.Euler(moveDistance.y * m_rotationSpeed,
                                                    moveDistance.x * m_rotationSpeed,
                                                   0);
        //transform.Rotate(new Vector3(moveDistance.x, moveDistance.y, 0f));

        m_pastMousePosition = m_currentMousePosition;
    }

    private void AdjustCameraPosition()
    {

    }

    //private void AdjustCameraDirection()
    //{

    //}
}
