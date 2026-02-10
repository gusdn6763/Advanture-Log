using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public class SubStatRule
{
    public LocalizedString statName;    //이름
    public string displayType;          //% 유무
    public float maxValue;              //서브 스탯 최대치
}