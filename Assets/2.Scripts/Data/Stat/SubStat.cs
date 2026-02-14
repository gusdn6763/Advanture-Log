using System;
using UnityEngine;

[Serializable]
public class SubStat : Stat
{
    public SubStatType Type { get; }

    private StatDisplayType displayType;
    private float maxValue = float.MaxValue;

    public override float BaseValue { get => base.BaseValue; set { base.BaseValue = Mathf.Clamp(value, 0f, maxValue); } }
    public override float FinalValue { get => Mathf.Clamp(base.finalValue, 0f, maxValue); }
    public string DisplayValue { get => StringUtil.FormatValueForDisplay(finalValue, displayType); }

    public SubStat(SubStatType type, float value)
    {
        Type = type;

        StatRuleSo statRuleSo = Managers.Data.StatRule;

        if (!statRuleSo.TryGet(type, out SubStatRule rule))
        {
            Debug.LogError($"[SubStat] StatRule 없음: {type}");
            return;
        }

        name = rule.StatName;
        displayType = rule.DisplayType;
        maxValue = rule.MaxValue;

        BaseValue = Mathf.Clamp(value, 0f, maxValue);
    }
}
