using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class ActorEntitySo : BaseEntitySo
{
    [SerializedDictionary("메인 스탯", "값")]
    [SerializeField] private SerializedDictionary<MainStatType, float> mainStatDic = new SerializedDictionary<MainStatType, float>();

    [SerializedDictionary("서브 스탯", "값")]
    [SerializeField] private SerializedDictionary<SubStatType, float> subStatDic = new SerializedDictionary<SubStatType, float>();

    public IReadOnlyDictionary<MainStatType, float> MainStatDic => mainStatDic;
    public IReadOnlyDictionary<SubStatType, float> SubStatDic => subStatDic;
}