using UnityEngine;

public class EnemyStatusManager : HPManager
{
    [Header("ストレージの参照")]
    private EnemyStatusStorage enemyStatusStorage;
    private int armorHp;
    private float currentarmorHP;
    private int Score;

    [Header("外装オブジェクト・コライダーの参照")]
    [SerializeField] private GameObject armorObject;

    [Header("エネミーID")]
    [SerializeField] public string enemyID = "";

    protected override void Awake()
    {
        base.Awake();

        EnemyStatsData stats = enemyStatusStorage.GetEnemyStats(enemyID);

        if(stats != null )
        {
            maxHp = stats.Mainhp;
            currentHp=stats.Mainhp;
            armorHp=stats.Armorhp;
            currentarmorHP = stats.Armorhp;

            Strength = stats.str;
            Defense = stats.def;
            Agility = stats.agi;
            Critical = stats.cri;
            CriticalDamage = stats.criDamage;
            Score = stats.score;
        }
    }
    public override void TakeDamage(float damage, float multiplier)
    {
        if (IsDead) return;

        float finalDamage = damage * multiplier;

        // 外装HPが残っている場合
        if (currentarmorHP > 0f)
        {
            currentarmorHP -= finalDamage;
            currentarmorHP = Mathf.Clamp(currentarmorHP, 0f, armorHp);
            Debug.Log($"{gameObject.name} の外装に {finalDamage} のダメージ (残り外装HP: {currentarmorHP})");

            // 外装がゼロになった瞬間
            if (currentarmorHP <= 0f)
            {
                Debug.Log($"{gameObject.name} の外装が破壊された！");
                if (armorObject != null) armorObject.SetActive(false);
            }
            return; 
        }

        // 外装無い敵用(まあいないと思うけど)
        base.TakeDamage(damage, multiplier);
    }
}
