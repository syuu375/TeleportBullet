using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTest : MonoBehaviour
{
    private Transform m_transform;
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_turningSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        m_transform = this.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current.wKey.isPressed)
        {
            m_transform.position += transform.forward * m_moveSpeed * Time.deltaTime;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            m_transform.position += -transform.forward * m_moveSpeed * Time.deltaTime;
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            m_transform.rotation *= Quaternion.Euler(0, -m_turningSpeed * Time.deltaTime, 0);
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            m_transform.rotation *= Quaternion.Euler(0, m_turningSpeed * Time.deltaTime, 0);
        }
    }
}
