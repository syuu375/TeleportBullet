using UnityEngine;

public class EnemyStatusManager : StatusManager
{
    [Header("ストレージの参照")]
    [SerializeField] private EnemyStatusStorage enemyStatusStorage;
    private int armorHp;
    private float currentarmorHP;
    private int Score;

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
}
