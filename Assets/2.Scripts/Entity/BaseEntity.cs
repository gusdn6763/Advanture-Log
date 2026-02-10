using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public abstract class BaseEntity : InitBase
{
    public BaseEntitySo BaseData { get; private set; }

    protected BoxCollider2D box;
    protected SpriteRenderer sp;

    public List<ActionMenuSo> ActionMenuList { get => BaseData.ActionMenus; }
    public Vector2 Position { get => transform.position; }
    public Vector2 Size { get => box.size; }

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
        BaseData = data;

        sp.sprite = BaseData.EntityImage;
        box.size = sp.sprite.bounds.size;
        box.offset = Vector2.zero;
    }
}