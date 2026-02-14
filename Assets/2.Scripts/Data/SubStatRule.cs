using System;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public class SubStatRule
{
    [SerializeField] private LocalizedString statName;    //이름
    [SerializeField] private StatDisplayType displayType; //% 유무
    [SerializeField] private float maxValue;              //서브 스탯 최대치

    public LocalizedString StatName { get => statName; }
    public StatDisplayType DisplayType { get => displayType; }
    public float MaxValue { get => maxValue; }
}