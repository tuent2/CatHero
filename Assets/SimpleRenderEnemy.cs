using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SimpleRenderEnemy : MonoBehaviour
{
    [SerializeField] List<GameObject> EnemyData;
    [SerializeField] bool isLooping;
    [SerializeField] EnemyPooling enemyPooling;
    [SerializeField] List<Transform> SpawnList;

    void Start()
    {
        StartCoroutine(SpawnEnemyWaves());
    }
    IEnumerator SpawnEnemyWaves()
    {
        yield return new WaitForSeconds(1.5f);
        do
        {

            //currentLv = wave;
            for (int i = 0; i < EnemyData.Count; i++)
            {
                
                var enemyPoolC = enemyPooling.GetObjectInPooling();
                int a = Random.Range(0,SpawnList.Count);
                enemyPoolC.transform.position = SpawnList[a].position;
                enemyPoolC.enabled = true;
                
                yield return new WaitForSeconds(Random.Range(0.75f,3f));
            }
            yield return new WaitForSeconds(2f);
            
        }
        while (isLooping == true);
    }
}
