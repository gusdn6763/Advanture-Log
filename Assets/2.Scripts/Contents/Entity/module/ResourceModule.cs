using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceModule
{
    public event Action<float> OnHpChanged;
    public event Action<float> OnMpChanged;

    [SerializedDictionary("메인 스탯", "값")]
    protected SerializedDictionary<MainStatType, MainStat> mainStatDic = new SerializedDictionary<MainStatType, MainStat>();

    [SerializedDictionary("서브 스탯", "값")]
    protected SerializedDictionary<SubStatType, SubStat> subStatDic = new SerializedDictionary<SubStatType, SubStat>();

    public float CurrentHp { get; private set; }
    public float CurrentMp { get; private set; }

    public ResourceModule(ActorEntitySo so)
    {
        foreach (KeyValuePair<MainStatType, float> kv in so.MainStatDic)
        {
            MainStat main = new MainStat(kv.Key, kv.Value);

            mainStatDic[kv.Key] = main;
            main.OnChanged += _ => RebuildSubStat();
        }

        foreach (KeyValuePair<SubStatType, float> kv in so.SubStatDic)
        {
            SubStat sub = new SubStat(kv.Key, kv.Value);

            subStatDic[kv.Key] = sub;

            //if (kv.Key == SubStatType.MaxHealth)
            //    sub.OnChanged += OnMaxHpChanged;

            //if (kv.Key == SubStatType.MaxMana)
            //    sub.OnChanged += OnMaxMpChanged;
        }

        // 4) Current 리소스 초기화: 보통 풀로 시작
        CurrentHp = GetStatValue(SubStatType.MaxHealth);
        CurrentMp = GetStatValue(SubStatType.MaxMana);
    }

    #region 스탯
    public float GetStatValue(MainStatType type)
    {
        if (mainStatDic.TryGetValue(type, out MainStat stat))
            return stat.FinalValue;
        else
            Debug.LogError($"해당 타입의 속성 없음: {type}");

        return 0f;
    }
    public float GetStatValue(SubStatType type)
    {
        if (subStatDic.TryGetValue(type, out SubStat stat))
            return stat.FinalValue;
        else
            Debug.LogError($"해당 타입의 속성 없음: {type}");

        return 0f;
    }

    public void AddModifier(MainStatType type, StatModifier modifier)
    {
        if (mainStatDic.TryGetValue(type, out MainStat stat))
            stat.AddModifier(modifier);
        else
            Debug.LogError($"해당 타입의 속성 없음: {type}");
    }
    public void AddModifier(SubStatType type, StatModifier modifier)
    {
        if (subStatDic.TryGetValue(type, out SubStat stat))
            stat.AddModifier(modifier);
        else
            Debug.LogError($"해당 타입의 속성 없음: {type}");
    }

    public bool RemoveModifier(MainStatType type, object source)
    {
        if (!mainStatDic.TryGetValue(type, out MainStat stat))
            return false;

        return stat.RemoveModifierByObject(source);
    }
    public bool RemoveModifier(SubStatType type, object source)
    {
        if (!subStatDic.TryGetValue(type, out SubStat stat))
            return false;

        return stat.RemoveModifierByObject(source);
    }

    #endregion

    #region 체력 & 마나
    public void AddHp(float delta, bool silent = false)
    {
        float next = Mathf.Clamp(CurrentHp + delta, 0f, GetStatValue(SubStatType.MaxHealth));

        CurrentHp = next;
        OnHpChanged?.Invoke(CurrentHp);
    }

    public void AddMp(float delta, bool silent = false)
    {
        float next = Mathf.Clamp(CurrentMp + delta, 0f, GetStatValue(SubStatType.MaxMana));

        CurrentMp = next;
        OnMpChanged?.Invoke(CurrentMp);
    }
    #endregion

    private void RebuildSubStat()
    {
        float prevMaxHp = GetStatValue(SubStatType.MaxHealth);
        float prevMaxMp = GetStatValue(SubStatType.MaxMana);

        foreach (KeyValuePair<MainStatType, MainStat> mainPair in mainStatDic)
        {
            MainStatType mainType = mainPair.Key;
            MainStat main = mainPair.Value;

            foreach (KeyValuePair<SubStatType, float> rule in main.SubStatPerPointDic)
            {
                SubStatType subType = rule.Key;
                float perPoint = rule.Value;
                float add = main.FinalValue * perPoint;

                SubStat sub;
                if (!subStatDic.TryGetValue(subType, out sub))
                    continue;

                sub.ReplaceMainStatValue(mainType, add);
            }
        }
    }
}