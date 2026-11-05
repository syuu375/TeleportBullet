using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemyStatsData
{
    public string id;//エネミー名
    public int Mainhp;//本体HP
    public int Armorhp;//外装HP
    public int str;//攻撃力
    public int def;//防御
    public int agi;//素早さ
    public int cri;//クリティカル率
    public float criDamage;//クリティカルダメージ
    public int score;//スコア
}

public class EnemyStatusStorage : MonoBehaviour
{
    [Header("CSVファイル(enemy)")]
    [SerializeField] private TextAsset csvFile;

    private List<EnemyStatsData> enemyStatsList = new List<EnemyStatsData>();

    void Awake()
    {
        LoadCSV();
    }

    void LoadCSV()
    {
        if (csvFile == null)
        {
            Debug.LogError("csvファイルがない");
            return;
        }
        string[] lines = csvFile.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] valuse = lines[i].Split(',');

            if (valuse.Length < 9) continue;

            EnemyStatsData data = new EnemyStatsData();
            data.id = valuse[0];
            data.Mainhp = int.Parse(valuse[1]);
            data.Armorhp = int.Parse(valuse[2]);
            data.str = int.Parse(valuse[3]);
            data.def = int.Parse(valuse[4]);
            data.agi = int.Parse(valuse[5]);
            data.cri = int.Parse(valuse[6]);
            data.criDamage = float.Parse(valuse[7]);
            data.score = int.Parse(valuse[8]);

            enemyStatsList.Add(data);

            Debug.Log($"Loaded Player ID:{data.id},MainHP:{data.Mainhp}");
        }
    }
    public EnemyStatsData GetEnemyStats(string targetid)
    {
        foreach (var date in enemyStatsList)
        {
            if (date.id == targetid)
            {
                return date;
            }
        }
        Debug.LogWarning($"指定されたIDのエネミーデータが見つかりません:{targetid}");
        return null;
    }
}
    
