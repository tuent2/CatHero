using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class LevelController : MonoBehaviour
{
    
    LvData CurrentLvData;
    [SerializeField] int currentLv;
    [SerializeField] int currentWave;
    //[SerializeField] int currentNumberEnemy;
    [SerializeField] List<Transform> SpawnList;
    [SerializeField] List<LvData> LvData;

    //[SerializeField] int;
    [SerializeField] List<EnemyPooling> enemyPooling;    
    //[SerializeField] float TimeBetweenWaves;
    [SerializeField] List<GameObject> EnemyList;

    public void Start()
    {
        StartRenderLv();
    }
    public void StartRenderLv()
    {
        CurrentLvData = LvData[currentLv - 1];
        
        //currentNumberEnemy = CurrentLvData.waves[0].enemies[0].enemyCount;
        StartCoroutine( GenEnemyPerWayOfTurn());
    }

    public IEnumerator GenEnemyPerWayOfTurn()
    {
        for (int i = 0; i < CurrentLvData.waves[currentWave].enemies.Count; i++)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 6f));
            for (int j = 0; j < CurrentLvData.waves[currentWave].enemies[i].numberOfEnemy; j++)
            {
                int EnemyIndex = GetIndexFromEnemyName(CurrentLvData.waves[currentWave].enemies[i].enemyNameId);
                var enemyPoolC = enemyPooling[EnemyIndex].GetObjectInPooling();
                int randomPosition = UnityEngine.Random.Range(0, SpawnList.Count);
                enemyPoolC.transform.position = SpawnList[randomPosition].position;
                enemyPoolC.enabled = true;
                if (i == CurrentLvData.waves[currentWave].enemies.Count - 1 && j == CurrentLvData.waves[currentWave].enemies[i].numberOfEnemy - 1)
                {

                    currentWave++;
                    Debug.Log(currentWave);
                    if (currentWave > CurrentLvData.waves.Count-1)
                    {
                        currentWave = 0;
                        currentLv++;
                        Debug.Log("Lên lv");
                    }
                    //Reset lai tu dau
                    if (currentLv > LvData.Count)
                    {
                        currentLv = 1;
                        currentWave = 0;
                        Debug.Log("Reset lai");
                    }
                    StartRenderLv();
                    break;
                }
                yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 1.5f));
                
            }
        }



    }

   


    public int GetIndexFromEnemyName(EnemyName enemyName)
    {
       
        string[] enumNames = Enum.GetNames(typeof(EnemyName));

        
        int index = Array.IndexOf(enumNames, enemyName.ToString());

        return index;
    }



}


