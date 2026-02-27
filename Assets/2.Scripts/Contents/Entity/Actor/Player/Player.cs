using System;

public class Player : ActorEntity
{
    public PlayerSo PlayerData { get; private set; }

    public string PlayerName { get; private set; }
    public int Fame { get; private set; } = 0;
    public int WeaponMastery { get; private set; } = 0;
    public int MaxCarryWeight { get; private set; } = 0;

    public NeedModule Need { get; private set; }

    public InventoryModule Inventory { get; private set; }

    public override bool Init()
    {
        if (!base.Init())
            return false;

        Need = new NeedModule(Buffs);
        Quest = new QuestModule();

        return true;
    }


    public void SetInfo(GameStartData data)
    {

    }

    public void Damaged()
    {
        if (true)
        {
            //특정 조건이면 색상 변경
        }
    }

    #region 퀘스트 구현 예정
    public QuestModule Quest { get; private set; }
    public void AcceptQuest()
    {

    }
    public void SetQuestReward()
    {

    }

    #endregion
}