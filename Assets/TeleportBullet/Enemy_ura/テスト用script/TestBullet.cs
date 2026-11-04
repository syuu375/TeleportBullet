using UnityEngine;

/// <summary>
/// テスト用の弾（Pushしない）。
/// DefensePart に当たったら TakeHit を呼び、レイヤー判定の結果をConsoleに出す。
/// </summary>
public class TestBullet : MonoBehaviour
{
    public float speed = 8f;
    public float damage = 10f;
    public float multiplier = 1f;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<DefensePart>(out var part)) return;

        bool accepted = part.TakeHit(gameObject, damage, multiplier);
        string layerName = LayerMask.LayerToName(gameObject.layer);
        string result = accepted ? "ダメージを受けた" : "無視された";

        Debug.Log($"[{layerName}] の弾 → {part.name}：{result}（HP {part.CurrentHp}/{part.MaxHp}）");
        Destroy(gameObject);
    }
}