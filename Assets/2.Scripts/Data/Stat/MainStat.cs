using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MainStat : Stat
{
    public IReadOnlyDictionary<SubStatType, float> SubStatPerPointDic { get; }

    public MainStat(MainStatType type, float value)
    {
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
