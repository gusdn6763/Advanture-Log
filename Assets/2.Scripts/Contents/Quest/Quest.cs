using System.Collections.Generic;

public class CollectObjectiveState
{
    public int CurrentAmount { get; }
}

[System.Serializable]
public class Quest
{
    public Quest(QuestSO data)
    {
        Data = data;
    }

    public QuestSO Data { get; }
    public List<KillObjective> Objectives { get; }

    public string Title => Data.Title.GetLocalizedString();
    public string Description => Data.Description.GetLocalizedString();
    public RankType Rank => Data.Rank;
    public QuestReward Reward => Data.Reward;
    public int TimeRemaining { get; set; }     // 남은 시간 런타임 소유

    //퀘스트에서 요구하는 아이템들을 체크함
    public bool IsComplete
    {
        get
        {
            //bool collectAll = CollectObjectives.TrueForAll(o => o.IsComplete);
            //bool killAll = KillObjectives.TrueForAll(o => o.IsComplete);

            //return collectAll && killAll;
            return true;
        }
    }
}

