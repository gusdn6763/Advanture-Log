using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MainStatRule
{
    [Header("메인 스탯별 서브 스탯 증가량")]
    [SerializeField] private SerializedDictionary<SubStatType, float> subStatPerPointDic = new SerializedDictionary<SubStatType, float>();

    public IReadOnlyDictionary<SubStatType, float> SubStatPerPointDic { get => subStatPerPointDic; }
}