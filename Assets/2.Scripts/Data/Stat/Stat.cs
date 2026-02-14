using System;
using System.Collections.Generic;
using UnityEngine.Localization;

public abstract class Stat
{
    protected List<StatModifier> modifiers = new List<StatModifier>();

    protected LocalizedString name;
    protected float baseValue;
    protected float finalValue;

    public string Name { get => name.GetLocalizedString(); }
    public virtual float BaseValue { get => baseValue; set { baseValue = value; CalculateFinalValue(); } }
    public virtual float FinalValue { get => finalValue; }

    #region 계산 로직
    private float GetModifierSums(StatCalculateType StatCalculateType)
    {
        float sum = 0;
        foreach (StatModifier modifier in modifiers)
        {
            if (StatCalculateType == modifier.Type)
                sum += modifier.Value;
        }

        return sum;
    }

    private void CalculateFinalValue()
    {
        float sumFlat = GetModifierSums(StatCalculateType.Flat);
        float sumAdd = GetModifierSums(StatCalculateType.PercentAdd);
        float sumMult = GetModifierSums(StatCalculateType.PercentMult);

        float adjustedBase = baseValue * (1f + sumAdd);
        finalValue = (adjustedBase + sumFlat) * (1f + sumMult);
    }
    #endregion

    public void AddModifier(StatModifier mod)
    {
        modifiers.Add(mod);
        CalculateFinalValue();
    }

    public bool RemoveModifierByObject(object obj)
    {
        int removed = modifiers.RemoveAll(m => m != null && m.Source == obj);
        if (removed > 0)
        {
            CalculateFinalValue();
            return true;
        }
        return false;
    }

    public bool RemoveAllModifiersFromSource(object source)
    {
        bool removed = false;
        for (int i = modifiers.Count - 1; i >= 0; i--)
        {
            if (modifiers[i].Source == source)
            {
                modifiers.RemoveAt(i);
                removed = true;
            }
        }

        if (removed)
            CalculateFinalValue();

        return removed;
    }
}