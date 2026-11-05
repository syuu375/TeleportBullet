using UnityEngine;

public class PlayerStatusManager : StatusManager
{
    [Header("ストレージの参照")]
    [SerializeField] private PlayerStatusStorage playerStatusStorage;

    [Header("プレイヤーID")]
    [SerializeField] public string playerID = "";

    protected override void Awake()
    {
        base.Awake();

        PlayerStatsData stats = playerStatusStorage.GetPlayerStats(playerID);

        if (stats != null)
        {
            maxHp = stats.hp;
            currentHp = stats.hp;

            Strength = stats.str;
            Defense = stats.def;
            Agility = stats.agi;
            Critical = stats.cri;
            CriticalDamage = stats.criDamage;
        }
    }
}
