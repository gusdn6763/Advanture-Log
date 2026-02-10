using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public class RankTier
{
    public LocalizedString rankName;
    public RankType rank;
    public int requestContribution;
}

[Serializable]
public class NeedRange
{
    public EffectSo effect;
    public NeedTier tier;
    [Range(0, 100)] public float minValue;
    [Range(0, 100)] public float maxValue;
}

[Serializable]
public class NeedData
{
    [SerializeField] private NeedType type;
    [SerializeField] private List<NeedRange> needRange = new List<NeedRange>();

    [Header("최대/최소/시작값")]
    [SerializeField] private float maxValue = 100f;
    [SerializeField] private float minValue = 0f;
    [SerializeField] private float startValue = 100f;

    [Header("자동 변화량(분/단위)")]
    [SerializeField] private float reducePerMinute = 1f;

    public NeedType Type => type;
    public float MaxValue => maxValue;
    public float MinValue => minValue;
    public float StartValue => startValue;
    public float ReducePerMinute => reducePerMinute;

    public NeedTier GetTierFromValue(float value)
    {
        foreach (NeedRange need in needRange)
        {
            if (value >= need.minValue && value <= need.maxValue)
                return need.tier;
        }
        return NeedTier.Normal;
    }

    public EffectSo GetEffectFromTier(NeedTier tier)
    {
        foreach (NeedRange need in needRange)
        {
            if (need.tier == tier)
                return need.effect;
        }
        return null;
    }
}

[CreateAssetMenu(menuName = "Game/Rule/PlayerRule", fileName = "PlayerRuleSo")]
public class PlayerRuleSo : ScriptableObject
{
    [Header("초기 시작 스탯")]
    [SerializeField] private int startStat;

    [Header("초기 설정 가능한 스탯 포인트")]
    [SerializeField] private int startPoint;

    [Header("레벨업 스탯")]
    [SerializeField] private int levelUpPoint;

    [Header("욕구")]
    [SerializeField] private List<NeedData> needRules = new List<NeedData>();

    [Header("등급")]
    [SerializeField] private List<RankTier> rankTiers = new List<RankTier>();


    public int StartStat { get => startStat; }
    public int StartPoint { get => startPoint; }
    public int LevelUpPoint { get => levelUpPoint; }
    public IReadOnlyList<NeedData> NeedRules { get => needRules; }
    public IReadOnlyList<RankTier> RankTiers { get => rankTiers; }
}