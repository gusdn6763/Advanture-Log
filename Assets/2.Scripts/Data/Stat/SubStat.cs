using System;
using UnityEngine;

[Serializable]
public class SubStat : Stat
{
    private StatDisplayType displayType;
    private float maxValue = float.MaxValue;

    public override float BaseValue { get => base.BaseValue; set { base.BaseValue = Mathf.Clamp(value, 0f, maxValue); } }
    public override float FinalValue { get => Mathf.Clamp(base.finalValue, 0f, maxValue); }
    public string DisplayValue { get => StringUtil.FormatValueForDisplay(finalValue, displayType); }

    public SubStat(SubStatType type, float value)
    {
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

    public void ReplaceMainStatValue(MainStatType mainType, float value)
    {
        //이벤트를 여러 번 불러오지 않기 위해 따로 할당
        modifiers.RemoveAll(m => m != null && Equals(m.Source, mainType));

        modifiers.Add(new StatModifier(value, CalculateType.Flat, mainType));

        CalculateFinalValue();
    }
}
