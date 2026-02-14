using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game/Rule/StatRule", fileName = "StatRuleSo")]
public class StatRuleSo : ScriptableObject
{
    [SerializeField] private SerializedDictionary<MainStatType, MainStatRule> mainStatDic = new SerializedDictionary<MainStatType, MainStatRule>();
    [SerializeField] private SerializedDictionary<SubStatType, SubStatRule> subStatDic = new SerializedDictionary<SubStatType, SubStatRule>();

    public IReadOnlyDictionary<MainStatType, MainStatRule> MainStatDic { get => mainStatDic; }
    public IReadOnlyDictionary<SubStatType, SubStatRule> SubStatDic { get => subStatDic; }

    public bool TryGet(MainStatType type, out MainStatRule mainStatRule)
    {
        if (!mainStatDic.TryGetValue(type, out mainStatRule))
        {
            Debug.LogError("할당되지 않은 메인 스탯 데이터 존재");
            return false;
        }
        return true;
    }

    public bool TryGet(SubStatType type, out SubStatRule subStatRule)
    {
        if (!subStatDic.TryGetValue(type, out subStatRule))
        {
            Debug.LogError("할당되지 않은 서브 스탯 데이터 존재");
            return false;
        }
        return true;
    }
}