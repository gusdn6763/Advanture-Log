using System.Collections.Generic;
using UnityEngine;

public class QuestRuleSO : ScriptableObject
{
    [SerializeField] private List<QuestSO> quests = new List<QuestSO>();
    [SerializeField] private int maxCount;                                  //받을 수 있는 퀘스트 최대 갯수

    public List<QuestSO> Quests => quests;
    public int MaxCount => maxCount;
}