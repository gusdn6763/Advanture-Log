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
        this.playerName = playerName;
        this.playerType = playerType;
        this.playerStat = playerStat;
    }
}

[Serializable]
public class GameSaveData
{
    public PlayerData playerData;
}