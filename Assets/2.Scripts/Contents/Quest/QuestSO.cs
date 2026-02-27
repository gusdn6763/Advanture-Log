using UnityEngine.Localization;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 퀘스트 보상
/// </summary>
public class QuestReward
{

}

/// <summary>
/// 퀘스트에서 요구하는 아이템
/// </summary>
[System.Serializable]
public class CollectObjective
{
    [SerializeField] private ItemEntitySo requestItem;
    [SerializeField] private int amount;

    public ItemEntitySo RequestItem => requestItem;
    public int Amount => amount;
}

/// <summary>
/// 퀘스트에서 요구하는 몹 킬
/// </summary>
[System.Serializable]
public class KillObjective
{
    [SerializeField] private MonsterSo requestMob;
    [SerializeField] private int amount;

    public MonsterSo RequestMob => requestMob;
    public int Amount => amount;
}

[CreateAssetMenu(menuName = "Quest/Quest")]
public class QuestSO : ScriptableObject
{
    [SerializeField] private LocalizedString title;                 //이름
    [SerializeField] private LocalizedString descripiton;           //상세설명
    [SerializeField] private List<CollectObjective> collectObjectives;  //아이템 수집
    [SerializeField] private List<KillObjective> killObjectives;        //몹죽이기
    [SerializeField] private int time;                              //기간
    [SerializeField] private RankType rank;                         //수락 조건
    [SerializeField] private QuestReward reward;                    //공적치

    public LocalizedString Title => title;
    public LocalizedString Description => descripiton;
    public IReadOnlyList<CollectObjective> MyCollectObjectives => collectObjectives;
    public IReadOnlyList<KillObjective> MyKillObjectives => killObjectives;
    public int Time => time;
    public RankType Rank => rank;
    public QuestReward Reward => reward;
}