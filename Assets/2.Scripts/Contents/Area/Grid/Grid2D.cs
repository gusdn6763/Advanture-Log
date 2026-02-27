using System.Collections.Generic;
using UnityEngine;

public class Grid2D
{
    private List<BaseEntity> entities = new List<BaseEntity>();
    public Vector2Int CellPos { get; private set; }
    public IReadOnlyList<BaseEntity> Entities { get { return entities; } }
    public int Count { get { return entities.Count; } }

    public bool IsBlocked
    {
        get
        {
            for (int i = 0; i < entities.Count; i++)
            {
                BaseEntity e = entities[i];
                if (e == null)
                    continue;

                if (e.Block)
                    return true;
            }
            return false;
        }
    }

    public Grid2D(Vector2Int cellPos)
    {
        CellPos = cellPos;
    }

    public BaseEntity Top
    {
        get
        {
            if (entities.Count == 0)
                return null;

            return entities[entities.Count - 1];
        }
    }

    public void Push(BaseEntity entity)
    {
        entities.Add(entity);
    }

    public bool Remove(BaseEntity entity)
    {
        return entities.Remove(entity);
    }
}