using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class BaseEntity : InitBase
{
    protected BaseEntitySO BaseData { get; private set; }

    protected BoxCollider2D box;
    protected SpriteRenderer sp;

    public Vector2 Position { get { return transform.position; } }

    public override bool Init()
    {
        if (!base.Init())
            return false;

        box = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();

        return true;
    }
}