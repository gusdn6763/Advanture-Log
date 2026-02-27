
using Data;
using UnityEngine;

public class MobEntity : ActorEntity
{
    public MonsterSo MonsterData { get; private set; }
    [Header("Mob-È®ÀÎ¿ë")]
    [SerializeField] private bool isDead;

    [SerializeField] protected EnemyState enemyState;

    public override bool Init()
    {
        if (!base.Init())
            return false;

        return true;
    }

    public override BaseSaveData SaveData()
    {
        var BaseSaveData = base.SaveData();

        return BaseSaveData;
    }

    public override bool LoadData(BaseSaveData BaseSaveData)
    {
        if (!base.LoadData(BaseSaveData))
            return false;

        if (BaseSaveData is MobSaveData mobSaveData)
        {
            return true;
        }
        else
            return false;
    }
}
