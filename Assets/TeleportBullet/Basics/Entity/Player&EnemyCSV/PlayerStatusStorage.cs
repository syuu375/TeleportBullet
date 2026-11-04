using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerStatsData
{
    public string id;//プレイヤー名
    public int hp;//HP
    public int str;//攻撃力
    public int def;//防御
    public int agi;//素早さ
    public int cri;//クリティカル率
    public float criDamage;//クリティカルダメージ
}

public class PlayerStatusStorage　:MonoBehaviour
{
    [Header("CSVファイル(TextAsset)")]
    [SerializeField] private TextAsset csvFile;

    private List<PlayerStatsData> playerStatsList=new List<PlayerStatsData>();
}