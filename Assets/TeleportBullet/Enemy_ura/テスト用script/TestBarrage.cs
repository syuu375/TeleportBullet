using UnityEngine;

/// <summary>
/// テスト用の弾幕（Pushしない）。
/// 扇状に弾を撃ち、弾ごとにレイヤーを順番に割り当てて、
/// DefensePart のレイヤー判定を確認する。
/// </summary>
public class TestBarrage : MonoBehaviour
{
    [Header("狙う相手")]
    [SerializeField] private Transform target;

    [Header("弾幕")]
    [SerializeField] private float interval = 1.5f;
    [SerializeField] private int bulletsPerShot = 6;
    [SerializeField] private float spreadAngle = 40f;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float damage = 10f;

    [Header("弾に割り当てるレイヤー（順番に使う）")]
    [SerializeField] private string[] layerNames = { "PlayerBullet", "EnemyBullet", "Default" };
    [SerializeField] private Color[] colors = { Color.cyan, Color.red, Color.gray };

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < interval) return;

        timer = 0f;
        Fire();
    }

    private void Fire()
    {
        if (target == null || layerNames.Length == 0) return;

        Vector3 baseDir = (target.position - transform.position).normalized;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            float t = bulletsPerShot == 1 ? 0.5f : i / (bulletsPerShot - 1f);
            float angle = Mathf.Lerp(-spreadAngle * 0.5f, spreadAngle * 0.5f, t);
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * baseDir;

            SpawnBullet(dir, i % layerNames.Length);
        }
    }

    private void SpawnBullet(Vector3 dir, int layerIndex)
    {
        int layer = LayerMask.NameToLayer(layerNames[layerIndex]);
        if (layer < 0)
        {
            Debug.LogWarning($"レイヤー「{layerNames[layerIndex]}」が存在しません");
            return;
        }

        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = $"TestBullet_{layerNames[layerIndex]}";
        go.layer = layer;
        go.transform.position = transform.position;
        go.transform.rotation = Quaternion.LookRotation(dir);
        go.transform.localScale = Vector3.one * 0.3f;

        // トリガーのColliderと、キネマティックなRigidbodyで OnTriggerEnter を発生させる
        go.GetComponent<Collider>().isTrigger = true;
        go.AddComponent<Rigidbody>().isKinematic = true;

        if (colors.Length > 0)
            go.GetComponent<Renderer>().material.color = colors[layerIndex % colors.Length];

        var bullet = go.AddComponent<TestBullet>();
        bullet.speed = bulletSpeed;
        bullet.damage = damage;
    }
}