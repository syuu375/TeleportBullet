using UnityEngine;

public class HPManager : MonoBehaviour,IDamage
{
    [Header("ステータス設定")]
     protected float maxHp;
     public int Strength;
     public int Defense;
     public int Agility;
     public int Critical;
     public float CriticalDamage;

    protected float currentHp;

    public bool IsDead { get; protected set; } = false;

    protected virtual void Awake()
    {
        currentHp = maxHp;
    }

    //IDamage 
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

    //死亡処理
    protected virtual void Die()
    {
        if (IsDead) return;
        IsDead = true;

        Debug.Log($"{gameObject.name} が死亡しました。");
        
    }
}
