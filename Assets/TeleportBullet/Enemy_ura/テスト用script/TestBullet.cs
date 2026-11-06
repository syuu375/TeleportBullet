using UnityEngine;

//テスト用の弾
public class TestBullet : MonoBehaviour
{
    public float speed = 8f;
    public float damage = 10f;
    public float multiplier = 1f;
    public float lifetime = 5f;

    [Tooltip("発射位置からこの距離だけ進んだら消える")]
    public float maxDistance = 15f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
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