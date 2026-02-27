using Data;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public abstract class BaseEntity : InitBase
{
    protected BaseEntitySo baseData;

    protected BoxCollider2D box;
    protected SpriteRenderer sp;

    public IReadOnlyList<ActionMenuSo> ActionMenuList { get => baseData.ActionMenus; }
    public bool Block { get => baseData.IsBlock; }

    public override bool Init()
    {
        if (!base.Init())
            return false;

        box = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();

        return true;
    }

    public virtual void SetInfo(BaseEntitySo data)
    {
        baseData = data;

        sp.sprite = baseData.EntityImage;
        box.size = sp.sprite.bounds.size;
        box.offset = Vector2.zero;
    }

    public virtual BaseSaveData SaveData()
    {
        BaseSaveData data = new BaseSaveData();

        data.Id = baseData.Id;

        return data;
    }

    public virtual bool LoadData(BaseSaveData BaseSaveData)
    {
        if (BaseSaveData is ActorSaveData actorSaveData)
        {
            return true;
        }
        else
            return false;
    }
}