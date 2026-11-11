using System;
using UnityEngine;

public class DefensePart : MonoBehaviour, IDamage
{
    [Header("HP")]
    [Tooltip("未設定なら親のEnemyStatusManagerを自動で探す（敵のIDを読むために使う）")]
    [SerializeField] private EnemyStatusManager status;
    

    private float maxHp;
    private bool hpInitialized;

    [Header("被ダメージ")]
    [Tooltip("この部位が受けるダメージの係数。硬い部位は1未満、脆い部位は1より大きくする")]
    [SerializeField] private float damageTakenRate = 1f;

    [Header("ダメージを受ける相手のレイヤー")]
    [Tooltip("ここで選んだレイヤーのオブジェクト（弾など）に当たったときだけダメージを受ける")]
    [SerializeField] private LayerMask damageSourceLayers;

    [Header("破壊対象（未設定なら子から自動取得）")]
    [SerializeField] private Collider[] colliders;
    [SerializeField] private Renderer[] renderers;

    public float MaxHp
    {
        get
        {
            EnsureHpInitialized();
            return maxHp;
        }
    }
    public float CurrentHp { get; private set; }
    public bool IsBroken { get; private set; }

    public event Action<DefensePart> Broken;

    private void Reset()
    {
        damageSourceLayers = LayerMask.GetMask("PlayerBullet", "EnemyBullet");
    }

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

        if (status == null) status = GetComponentInParent<EnemyStatusManager>();
    }

    // EnemyStatusStorage は Awake の中でCSVを読み込む。
    // 読み込みが終わる前に参照しないよう、全部の Awake が終わったあとの Start で読む。
    private void Start()
    {
        EnsureHpInitialized();
    }

    private void EnsureHpInitialized()
    {
        if (hpInitialized) return;

        RefreshMaxHp();
        CurrentHp = maxHp;
        hpInitialized = true;
    }

    private void RefreshMaxHp()
    {
        if (status == null)
        {
            Debug.LogError($"{name}: 親に EnemyStatusManager が見つかりません（敵のIDを読めません）", this);
            return;
        }
    }

    public void TakeDamage(float damage, float multiplier)
    {
        EnsureHpInitialized();

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

    public void ResetPart()
    {
        IsBroken = false;
        RefreshMaxHp();
        CurrentHp = maxHp;
        hpInitialized = true;
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