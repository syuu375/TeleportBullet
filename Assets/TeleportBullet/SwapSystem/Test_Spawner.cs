using UnityEngine;

public class Test_Spawner : MonoBehaviour
{
    //[SerializeField] private Middleman_Object middleman;
    [SerializeField] private Middleman_Object m_objPool;

    [SerializeField] private Vector3 m_spawnPosition;


    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            SetRandomPos();
            SpawnEnemy(m_spawnPosition, m_objPool, Enum_EnemyType.Prov01);
        }
    }

    /// <summary>
    /// spawn enemy from enemyPools
    /// </summary>
    /// <param name="objPools">objPools</param>
    private void SpawnEnemy(Vector3 position, Middleman_Object objPools, Enum_EnemyType enemyType)
    {
        var obj = objPools.GetComponent(enemyType);

        obj.gameObject.SetActive(true);
        obj.transform.position = position;
    }

    private void SetRandomPos()
    {
        m_spawnPosition.x = Random.Range(-4.0f, 4.0f);
        m_spawnPosition.y = Random.Range(0.0f, 3.0f);
        m_spawnPosition.z = Random.Range(-4.0f, 4.0f);
    }
}
