using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Game/Entity/Player", fileName = "Player")]
public class PlayerSo : ActorEntitySo
{
    [Header("기본 메인 스탯")]
    [SerializeField] private SerializedDictionary<MainStatType, int> baseMainStatDic;

    [Header("기본 서브 스탯")]
    [SerializeField] private SerializedDictionary<SubStatType, float> baseSubStatDic;

    [Header("기본 스킬")]
    [SerializeField] private List<SkillSo> baseSkillList;

    [Header("캐릭터 선택창 직업 설명")]
    [SerializeField] private LocalizedString jobDescription;

    [Header("플레이어 규칙")]
    [SerializeField] protected PlayerRuleSo playerRuleSo;

    public LocalizedString JobDescription => jobDescription; 
    public IReadOnlyDictionary<MainStatType, int> BaseMainStatDic => baseMainStatDic;
    public IReadOnlyDictionary<SubStatType, float> BaseSubStatDic => baseSubStatDic;
    public IReadOnlyList<SkillSo> BaseSkillList => baseSkillList;
}