using UnityEngine;
using UnityEngine.InputSystem;

//テスト用
public class DamageTester : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float multiplier = 1f;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Debug.Log("何にも当たらなかった");
            return;
        }

        var target = hit.collider.GetComponent<IDamage>();
        if (target == null)
        {
            Debug.Log($"{hit.collider.name} は IDamage を持っていない");
            return;
        }

        target.TakeDamage(damage, multiplier);
        Debug.Log($"{hit.collider.name} に {damage} x {multiplier} のダメージ");
    }
}