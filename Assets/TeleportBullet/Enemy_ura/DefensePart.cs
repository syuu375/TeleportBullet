using System;
using System.Reflection;
using UnityEngine;


//部位ごとのHPを持ち、HPが0になるとコライダーを無効化して非表示にする。
//子オブジェクトに専用Colliderを付けて使う。
public class DefensePart : MonoBehaviour, IDamage
{
    [Header("HP")]
    [Tooltip("未設定なら親のStatusManagerを自動で探す")]
    [SerializeField] private StatusManager status;

    private float maxHp;

    [Header("被ダメージ")]
    [Tooltip("この部位が受けるダメージの係数。硬い部位は1未満、脆い部位は1より大きくする")]
    [SerializeField] private float damageTakenRate = 1f;

    [Header("ダメージを受ける相手のレイヤー")]
    [Tooltip("ここで選んだレイヤーのオブジェクト（弾など）に当たったときだけダメージを受ける")]
    [SerializeField] private LayerMask damageSourceLayers;

    [Header("破壊対象（未設定なら子から自動取得）")]
    [SerializeField] private Collider[] colliders;
    [SerializeField] private Renderer[] renderers;

    public float MaxHp => maxHp;
    public float CurrentHp { get; private set; }
    public bool IsBroken { get; private set; }


    public event Action<DefensePart> Broken;


    private static readonly FieldInfo StatusMaxHpField =
        typeof(StatusManager).GetField("maxHp", BindingFlags.Instance | BindingFlags.NonPublic);

    private bool TryGetStatusMaxHp(out float value)
    {
        value = 0f;
        if (status == null || StatusMaxHpField == null) return false;

        value = (float)StatusMaxHpField.GetValue(status);
        return true;
    }

    private void Reset()
    {
        damageSourceLayers = LayerMask.GetMask("PlayerBullet", "EnemyBullet");
    }

    //相手（弾など）のレイヤーが、ダメージを受けてよいレイヤーかを判定する
    public bool CanBeDamagedBy(GameObject source)
    {
        return (damageSourceLayers.value & (1 << source.layer)) != 0;
    }

    public bool TakeHit(GameObject source, float damage, float multiplier)
    {
        if (!CanBeDamagedBy(source)) return false;

        TakeDamage(damage, multiplier);
        return true;
    }

    private void Awake()
    {
        if (colliders == null || colliders.Length == 0)
            colliders = GetComponentsInChildren<Collider>();

        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<Renderer>();

        if (status == null) status = GetComponentInParent<StatusManager>();

        if (TryGetStatusMaxHp(out float statusMaxHp))
        {
            maxHp = statusMaxHp;
        }
        else
        {
            Debug.LogError($"{name}: StatusManager からMaxHPを取得できませんでした", this);
        }

        CurrentHp = maxHp;
    }

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

    private void Break()
    {
        IsBroken = true;
        SetActiveParts(false);
        Broken?.Invoke(this);
    }

    //HPを全回復して部位を復活させる（リスポーンや再戦用）
    public void ResetPart()
    {
        IsBroken = false;
        CurrentHp = maxHp;
        SetActiveParts(true);
    }

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