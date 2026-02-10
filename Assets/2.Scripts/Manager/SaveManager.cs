using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string playerName;
    public string playerType;
    public List<int> playerStat;

    public PlayerData(string playerName, string playerType, List<int> playerStat)
    {

    }
}

[Serializable]
public class GameSaveData
{
    public int Gold = 0;

    public int ItemDbIdGenerator = 1;
    public PlayerData playerData;
}

public class SaveManager : MonoBehaviour
{
    public void Save()
    {

    }

    public void Load()
    {

    }
}
