using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public static class StatCalculator
{
    public static IReadOnlyDictionary<SubStatType, float> CalcTotalSubStats(IReadOnlyDictionary<MainStatType, int> mainTotals)
    {
        StatRuleSo rule = Managers.Data.StatRule;

        Dictionary<SubStatType, float> result = new Dictionary<SubStatType, float>();

        foreach (KeyValuePair<SubStatType, SubStatRule> kv in rule.SubStatDic)
        {
            SubStatType subType = kv.Key;
            SubStatRule subRule = kv.Value;

            float total = 0f;
            // subRule 구조에 맞게 구현 (예: sources 리스트)
            //foreach (var src in subRule.Sources)
            //{
            //    if (mainTotals.TryGetValue(src.MainType, out int v))
            //        total += v * src.Multiplier;
            //}

            result[subType] = total;
        }

        return result;
    }

    public static IReadOnlyDictionary<SubStatType, float> GetSubStatDicFromMainStat(MainStatType mainStatType, int mainStatValue)
    {
        StatRuleSo rule = Managers.Data.StatRule;

        Dictionary<SubStatType, float> resultDic = new Dictionary<SubStatType, float>();
        
        rule.TryGet(mainStatType, out MainStatRule mainStatRule);

        return mainStatRule.SubStatPerPointDic;
    }
}