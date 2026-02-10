using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game/Rule/StatRule", fileName = "StatRuleSo")]
public class StatRuleSo : ScriptableObject
{
    [SerializeField] private SerializedDictionary<MainStatType, MainStatRule> mainStatDic = new SerializedDictionary<MainStatType, MainStatRule>();
    [SerializeField] private SerializedDictionary<SubStatType, SubStatRule> subStatDic = new SerializedDictionary<SubStatType, SubStatRule>();

    public IReadOnlyDictionary<MainStatType, MainStatRule> MainStatDic { get => mainStatDic; }
    public IReadOnlyDictionary<SubStatType, SubStatRule> SubStatDic { get => subStatDic; }

    public bool TryGet(MainStatType type, out MainStatRule mainStatRule)
    {
        if (mainStatDic.TryGetValue(type, out mainStatRule))
            return true;

        return false;
    }
    public bool TryGet(SubStatType type, out SubStatRule subStatRule)
    {
        if (subStatDic.TryGetValue(type, out subStatRule))
            return true;

        return false;
    }
}