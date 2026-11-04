using UnityEngine;

/// <summary>
/// テスト用の弾（Pushしない）。
/// DefensePart に当たったら TakeHit を呼び、レイヤー判定の結果をConsoleに出す。
/// それ以外の IDamage（コアのテスト用など）に当たったら TakeDamage を呼ぶ。
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
        string layerName = LayerMask.LayerToName(gameObject.layer);

        if (other.TryGetComponent<DefensePart>(out var part))
        {
            bool accepted = part.TakeHit(gameObject, damage, multiplier);
            string result = accepted ? "ダメージを受けた" : "無視された";

            Debug.Log($"[{layerName}] の弾 → {part.name}：{result}（HP {part.CurrentHp}/{part.MaxHp}）");
            Destroy(gameObject);
            return;
        }

        if (other.TryGetComponent<IDamage>(out var target))
        {
            Debug.Log($"[{layerName}] の弾 → {other.name} に命中");
            target.TakeDamage(damage, multiplier);
            Destroy(gameObject);
        }
    }
}