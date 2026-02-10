using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public class MainStatRule
{
    [Header("메인 스탯별 서브 스탯 증가량")]
    [SerializeField] private SerializedDictionary<SubStatType, float> subStatPerPointDic = new SerializedDictionary<SubStatType, float>();
    [SerializeField] private LocalizedString statName;    //이름

    public IReadOnlyDictionary<SubStatType, float> SubStatPerPointDic { get => subStatPerPointDic; }
    public LocalizedString StatName { get => statName; }
}