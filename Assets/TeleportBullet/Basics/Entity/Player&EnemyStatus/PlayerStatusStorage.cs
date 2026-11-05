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
    [Header("CSVファイル(player)")]
    [SerializeField] private TextAsset csvFile;

    private List<PlayerStatsData> playerStatsList=new List<PlayerStatsData>();

     void Awake()
    {
        LoadCSV();   
    }
    void LoadCSV()
    {
        if(csvFile==null)
        {
            Debug.LogError("csvファイルがない");
            return;
        }
        //一列ごとの配列に
        string[] lines = csvFile.text.Split(new char[] { '\n','\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        for(int i=1;i<lines.Length;i++)
        {
            //各項目の文字列を取り出す
            string[] valuse = lines[i].Split(',');

            if (valuse.Length < 7) continue;

            PlayerStatsData data = new PlayerStatsData();
            data.id=valuse[0];
            data.hp=int.Parse(valuse[1]);
            data.str=int.Parse(valuse[2]);
            data.def=int.Parse(valuse[3]);
            data.agi=int.Parse(valuse[4]);
            data.cri=int.Parse(valuse[5]);
            data.criDamage=float.Parse(valuse[6]);

            playerStatsList.Add(data);

            Debug.Log($"Loaded Player ID:{data.id},HP:{data.hp}");
        }
    }
    public PlayerStatsData GetPlayerStats(string targetId)
    {
        foreach(var date in playerStatsList)
        {
            if(date.id==targetId)
            {
                return date;
            }
        }

        Debug.LogWarning($"指定されたIDのプレイヤーデータが見つかりませんでした:{targetId}");
        return null;
    }
}