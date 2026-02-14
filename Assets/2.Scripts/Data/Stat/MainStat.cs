using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MainStat : Stat
{
    public MainStatType Type { get; }

    public IReadOnlyDictionary<SubStatType, float> SubStatPerPointDic { get; }

    public MainStat(MainStatType type, float value)
    {
        Type = type;

        StatRuleSo statRuleSo = Managers.Data.StatRule;

        if (!statRuleSo.TryGet(type, out MainStatRule rule))
        {
            Debug.LogError($"[MainStat] StatRule ¾øÀ½: {type}");
            return;
        }

        name = rule.StatName;
        SubStatPerPointDic = rule.SubStatPerPointDic;
        BaseValue = value;
    }
}
