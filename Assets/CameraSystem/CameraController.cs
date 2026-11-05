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
    [SerializeField] private Vector2 m_targetPositionOnTheScreen;
    [SerializeField] private float m_sensitivity;

    //[Header("Rotation")]
    //[SerializeField] private float m_lookPlayDistance;
    //[SerializeField] private float m_followSpeed;
    //[SerializeField] private float m_rotationSpeed;
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
    [SerializeField] private Vector2 m_pastMousePosition;
    [SerializeField] private Vector2 m_moveDistance;
    [SerializeField] private float m_fAngleX;
    [SerializeField] private float m_fAngleY;

    [Header("Rotation")]
    [SerializeField] private Vector3 m_initialPosition;
    [SerializeField] private Quaternion m_initialRotation;

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
        m_fAngleX = 0;
        m_fAngleY = 0;
        
        m_initialPosition = transform.position;
        m_initialRotation = Quaternion.identity;
    }


    private void UpdateCameraPosition()
    {
        Vector3 horizontalVector = m_targetPosition - m_cameraPosition;
        horizontalVector.y = 0;
        float distance = horizontalVector.magnitude;

        //float moveDistance = 0;
        //if (distance > m_cameraDistance + m_cameraPlayDistance)
        //{
        //    moveDistance = distance - (m_cameraDistance + m_cameraPlayDistance);
        //    moveDistance *= Time.deltaTime * m_followSpeed;
        //}
        //else if (distance < m_cameraDistance + m_cameraPlayDistance)
        //{
        //    moveDistance = distance - (m_cameraDistance - m_cameraPlayDistance);
        //    moveDistance *= Time.deltaTime * m_leaveSpeed;
        //}

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


    private void RotationCamera()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        m_currentMousePosition = m_mouseCurrent.position.ReadValue();


        m_fAngleX += (m_currentMousePosition.x - m_pastMousePosition.x) * m_sensitivity;
        m_fAngleY += (m_currentMousePosition.y - m_pastMousePosition.y) * m_sensitivity;

        if (m_fAngleX > 360.0f) m_fAngleX -= 360.0f;
        if (m_fAngleY > 360.0f) m_fAngleY -= 360.0f;

        transform.rotation = m_initialRotation;
        transform.Rotate(-m_fAngleY * 360.0f / (2.0f * Mathf.PI), m_fAngleX * 360.0f / (2.0f * Mathf.PI), 0.0f, Space.World);

        m_pastMousePosition = m_currentMousePosition;
    }

    private void AdjustCameraPosition()
    {

    }

    //private void AdjustCameraDirection()
    //{

    //}

    private void Normalize(ref float value1, ref float value2)
    {
        float length = Mathf.Sqrt(value1 * value1 + value2 * value2);
        value1 /= length;
        value2 /= length;
    }
}
