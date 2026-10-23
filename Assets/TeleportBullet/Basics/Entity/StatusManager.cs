using UnityEngine;

public class StatusManager : MonoBehaviour,IDamage
{
    [Header("ステータス設定")]
    [SerializeField] protected float maxHp = 100f;
    protected float currentHp;

    public bool IsDead { get; protected set; } = false;

    protected virtual void Awake()
    {
        currentHp = maxHp;
    }

    // IDamage （部位倍率などを考慮）
    public virtual void TakeDamage(float damage, float multiplier)
    {
        if (IsDead) return;

        float finalDamage = damage * multiplier;
        currentHp -= finalDamage;
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp);

        Debug.Log($"{gameObject.name} に {finalDamage} のダメージ (残りHP: {currentHp})");

        if (currentHp <= 0f)
        {
            Die();
        }
    }

    // 死亡処理（子クラスでオーバーライドして個別の演出を追加）
    protected virtual void Die()
    {
        if (IsDead) return;
        IsDead = true;

        Debug.Log($"{gameObject.name} が死亡しました。");
        // TODO: 死亡時の処理（プールへの返却）
    }
}
