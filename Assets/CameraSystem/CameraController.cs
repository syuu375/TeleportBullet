using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform m_transform;
    private Vector3 m_lookPosition;
    private Vector3 m_direction;

    [Header("Component")]
    [SerializeField] private Transform m_target;

    [Header("Setting")]
    [SerializeField] private Vector3 m_cameraOffset;
    [SerializeField] private Vector3 m_targetOffset;
    //[SerializeField] private float m_cameraPlayDistance;
    //[SerializeField] private float m_acceleration;
    //[SerializeField] private float m_velocityConvergence;
    //[SerializeField] private float m_turningAcceleration;
    //[SerializeField] private float m_turningVelocityConvergence;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        m_transform = this.transform;
    }

    private void Update()
    {
        Vector3 cameraPosition = m_target.right * m_cameraOffset.x + m_target.up * m_cameraOffset.y + m_target.forward * m_cameraOffset.z;

        m_transform.position = m_target.position + cameraPosition;
        m_direction = m_target.transform.position - m_transform.position + m_targetOffset;

        m_transform.rotation = Quaternion.LookRotation(m_direction);
    }
}
