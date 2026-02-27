using Data;
using UnityEngine;

public abstract class ActorEntity : BaseEntity
{
    public ActorEntitySo ActorData { get; private set; }
    protected ResourceModule Resource { get; private set; }
    protected EquipmentModule Equipments { get; private set; }
    protected BuffModule Buffs { get; private set; }

    public override bool Init()
    {
        if (!base.Init())
            return false;

        Resource = new ResourceModule(ActorData);
        Equipments = new EquipmentModule(Resource);
        Buffs = new BuffModule(Resource);

        return true;
    }

    public override BaseSaveData SaveData()
    {
        ActorSaveData data = new ActorSaveData(base.SaveData());

        return data;
    }

    public override bool LoadData(BaseSaveData BaseSaveData)
    {
        if (!base.LoadData(BaseSaveData))
            return false;

        if (BaseSaveData is ActorSaveData actorSaveData)
        {
            return true;
        }
        else
            return false;
    }

    #region 전투
    /// <summary>
    /// 피격 공식: (공격력 * (1 + 상대 속성 친화력) - 방어력) * (1 - 속성 저항력)
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(int damageInfo)
    {
        // 0) 회피
        if (TryDodge())
        {
            //OnDodgeEvent?.Invoke(this);
            return;
        }

        // 2) 방어력
        float defenceValue = Resource.GetStatValue(SubStatType.MinPhysicalDamage);

        //float damageAfterDefense = Mathf.Max(0, damageInfo.amount - defenceValue);


        // 3) 속성 저항력
        //float finalDamage = CalculateResistance(damageAfterDefense, damageInfo.element);

        // 4) 버프 상태에 따른 경감?

        // 5) 최종 적용
        //if (finalDamage > 0f)
            //part.Damaged(finalDamage);
    }
    public virtual bool TryDodge()
    {
        float evasion = Resource.GetStatValue(SubStatType.Evasion);

        if (Random.value < Mathf.Clamp01(evasion))
            return true;
        else
            return false;
    }
    private void Die()
    {
        // 사망 처리
    }

    public void Test()
    {

    }
    #endregion

    #region 행동
    public void PlayerTimeAction(float time)
    {
        //OnTimeActionEvent?.Invoke(this, minutes);
        //Buffs?.Tick(minutes);
    }

    #endregion

    #region 이벤트
    //회피시
    //public Action<> OnDodgeEvent;

    #endregion


    // 1) 장비: 도메인 동사 + 규칙은 EquipmentModule가 처리
    public bool Equip(EquipmentEntity equipItem)
    {
        //equipItem.Equipments.Equip(so);                           // 목록/슬롯 상태 갱신

        //if (so.onEquipBundle.TryResolve(out var payload, out var def))
        //{
        //    if (payload.HasAny()) ApplyPayload(so, payload);      // 장착~탈착 유지
        //    if (def != null && def.duration > 0f) Buffs.Apply(def, source: so); // 지속형 버프는 BuffModule
        //}
        return true;
    }

    public void UnEquip(EquipmentEntity equipment)
    {
        //Equipments.Unequip(so);            // 슬롯/세트 상태 정리
        //RemoveAllFromSource(so);           // 스탯/친화/저항 모디파이어 회수
        //Buffs.RemoveAllFromSource(so);     // 장착 중 달았던 버프 제거
    }

    // 2) 소비(즉시 + 선택적 버프)
    //public void UseConsumable(ConsumableItemSO so)
    //{
    //    //if (so.instantPayload.HasAny()) ApplyPayload(so, so.instantPayload); // 즉시형
    //    //if (so.buffEffects != null)
    //    //    foreach (var def in so.buffEffects)
    //    //        Buffs.Apply(def, source: so); // 지속형은 BuffModule
    //    //Inventory.Consume(so); // 수량/쿨다운 등
    //}

    //// 3) 버프 직접 적용(스킬/행동 등)
    //public void ApplyBuff(EffectSO def, object source)
    //{
    //    //Buffs.Apply(def, source); // 지속/스택/상충/만료 전담
    //}
}