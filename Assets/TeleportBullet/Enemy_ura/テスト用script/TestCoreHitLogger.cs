using UnityEngine;

/// <summary>
/// テスト用（Pushしない）。コアのオブジェクトに付けて、弾が当たったことをConsoleに出す。
/// コアのColliderが有効なときだけ、ここに弾が届く。
/// </summary>
public class TestCoreHitLogger : MonoBehaviour, IDamage
{
    public void TakeDamage(float damage, float multiplier)
    {
        Debug.Log($"コア「{name}」に弾が当たった（{damage} x {multiplier}）");
    }
}