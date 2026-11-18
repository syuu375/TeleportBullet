using UnityEngine;

public class Test_Spawner : MonoBehaviour
{
    //[SerializeField] private Middleman_Object middleman;
    [SerializeField] private ObjectPoolManager m_pool;

    [SerializeField] private Vector3 m_spawnPosition;


    void Start()
    {
        //SpawnEnemy(0);
        SetRandomPos();
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        //transform.position = m_spawnPosition;

        GameObject obj = m_pool.GetObjectFromPool();

        obj.transform.position = m_spawnPosition;
    }

    private void SetRandomPos()
    {
        m_spawnPosition.x = Random.Range(-4.0f, 4.0f);
        m_spawnPosition.y = Random.Range(0.0f, 3.0f);
        m_spawnPosition.z = Random.Range(-4.0f, 4.0f);
    }

    //public void SpawnEnemy(int enemyType)
    //{
    //    var enemy = middleman.GetComponent((Enum_EnemyType)enemyType);

    //    float x = Random.Range(-4.0f, 4.0f);
    //    float y = Random.Range(0.0f, 3.0f);
    //    float z = Random.Range(-4.0f, 4.0f);
    //    enemy.transform.position = new Vector3(x, y, z);
    //    enemy.gameObject.SetActive(true);
    //}
}
