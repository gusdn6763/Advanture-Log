using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class ActorEntitySo : BaseEntitySo
{
    [SerializedDictionary("메인 스탯", "값")]
    [SerializeField] private SerializedDictionary<MainStatType, int> mainStatDic = new SerializedDictionary<MainStatType, int>();

    [SerializedDictionary("서브 스탯", "값")]
    [SerializeField] private SerializedDictionary<SubStatType, float> subStatDic = new SerializedDictionary<SubStatType, float>();

    [Header("기본 스킬")]
    [SerializeField] private List<SkillSo> baseSkillList;

    public IReadOnlyDictionary<MainStatType, int> MainStatDic => mainStatDic;
    public IReadOnlyDictionary<SubStatType, float> SubStatDic => subStatDic;

    public IReadOnlyList<SkillSo> BaseSkillList => baseSkillList;
}