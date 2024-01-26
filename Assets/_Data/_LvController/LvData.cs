using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LvData : ScriptableObject
{
    public int lv;
    public bool isHardLv;
    public List<WaveData> waves;
    
    public List<GiftData> giftDatas;
}

[System.Serializable]
public class GiftData
{
    public int GiftType;
    public int GiftNumber;
}


//[System.Serializable]
//public class TurnData
//{
//    public List<WaveData> waveDatas;
//}




[System.Serializable]
public class WaveData
{
    public List<EnemyData> enemies;
}


public enum EnemyName
{
    TypeA,
    TypeB,
    TypeC
}
[System.Serializable]
public class EnemyData
{
    public EnemyName enemyNameId;
    public int numberOfEnemy;
}
