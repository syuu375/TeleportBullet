using System;
using UnityEngine;

// 部位ごとのHPを持ち、HPが0になるとコライダーを無効化して非表示にする。
//子オブジェクトに専用Colliderを付けて使う。
public class DefensePart : MonoBehaviour, IDamage
{
    [Header("HP")]
    [SerializeField] private float maxHp = 50f;

    [Header("被ダメージ")]　//ダメージ倍率
    [SerializeField] private float damageTakenRate = 1f;

    [Header("破壊対象（未設定なら子から自動取得）")]
    [SerializeField] private Collider[] colliders;
    [SerializeField] private Renderer[] renderers;

    public float MaxHp => maxHp;
    public float CurrentHp { get; private set; }
    public bool IsBroken { get; private set; }

    //破壊された瞬間に呼ばれる。
    public event Action<DefensePart> Broken;

    //初期化処理
    private void Awake()
    {
        if (colliders == null || colliders.Length == 0)
            colliders = GetComponentsInChildren<Collider>();

        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<Renderer>();

        CurrentHp = maxHp;
    }

    //ダメージ処理
    public void TakeDamage(float damage, float multiplier)
    {
        if (IsBroken) return;

        float finalDamage = damage * multiplier * damageTakenRate;
        if (finalDamage <= 0f) return;

        CurrentHp = Mathf.Max(0f, CurrentHp - finalDamage);

        if (CurrentHp <= 0f)
        {
            Break();
        }
    }

    //破壊処理
    private void Break()
    {
        IsBroken = true;
        SetActiveParts(false);
        Broken?.Invoke(this);
    }

    //復活処理(リスポーンや再選時)
    public void ResetPart()
    {
        IsBroken = false;
        CurrentHp = maxHp;
        SetActiveParts(true);
    }

    //表示や判定の切り替え
    private void SetActiveParts(bool active)
    {
        foreach (var c in colliders)
        {
            if (c != null) c.enabled = active;
        }

        foreach (var r in renderers)
        {
            if (r != null) r.enabled = active;
        }
    }
}