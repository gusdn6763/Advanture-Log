using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestModule
{
    private List<Quest> acceptQuests = new List<Quest>();   //플레이어가 수락한 퀘스트들

    private RankType PlayerRank;

    public bool AcceptQuest(Quest quest)
    {
        //갯수 제한 + 플레이어 랭크 제한
        if (acceptQuests.Count < Managers.Quest.QuestMaxCount && quest.Rank >= PlayerRank)
        {
            //acceptQuests.Add(questScript);
            //questCountTxt.text = acceptQuests.Count + "/" + Managers.Data.QuestMaxCount;
            ////itemCountChangedEvent는 이벤트 함수로 아이템을 얻을때 실행한다.
            ////아이템을 얻을시 퀘스트에 필요한 아이템이면 갯수를 증가시키기 위함    
            //foreach (CollectObjective o in questScript.MyQuest.MyCollectObjectives)
            //{
            //    Player.Current.Inventory.OnItemCountChangedAction += o.UpdateItemCount;
            //    //퀘스트를 수락 후 인벤토리에 아이템이 있는지 체크  
            //    o.UpdateItemCount();
            //}

            //foreach (KillObjective o in questScript.MyQuest.MyKillObjectives)
            //    Managers.Game.KillConfirmed += o.UpdateKillCount;

            return true;
        }

        return false;
    }

    public void GiveUpQuest(Quest quest)
    {
        if (acceptQuests.Contains(quest))
        {
            //foreach (CollectObjective o in questScript.MyQuest.MyCollectObjectives)
            //    Player.Current.Inventory.OnItemCountChangedAction -= o.UpdateItemCount;

            //foreach (KillObjective o in questScript.MyQuest.MyKillObjectives)
            //    Managers.Game.KillConfirmed -= o.UpdateKillCount;

            RemoveQuest(quest);
        }
    }

    public void CompleteQuest(Quest quest)
    {
        if (acceptQuests.Contains(quest))
        {
            //foreach (CollectObjective o in questScript.MyQuest.MyCollectObjectives)
            //{
            //    Player.Current.Inventory.OnItemCountChangedAction -= o.UpdateItemCount;
            //    o.Complete();
            //}

            //foreach (KillObjective o in questScript.MyQuest.MyKillObjectives)
            //    Managers.Game.KillConfirmed -= o.UpdateKillCount;

            RemoveQuest(quest);
        }
        else
            Debug.LogError("수락하지 않은 퀘스트인데 퀘스트를 완료함");
    }
    public void RemoveQuest(Quest quest)
    {
        acceptQuests.Remove(quest);
    }
}