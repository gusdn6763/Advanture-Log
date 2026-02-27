using System;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    private AreaSo areaSo;
    private Grid2D[,] cells;

    private readonly List<BaseEntity> extraEntities = new List<BaseEntity>();

    public Sprite BackgroundSprite { get => areaSo.BackgroundSprite; }
    public Vector2Int Size { get => areaSo.Size; } 
    public Vector2 OriginWorld { get; private set; }      // (0,0)셀 좌표

    public void Init(AreaSo data)
    {
        areaSo = data;

        int width = areaSo.Size.x;
        int height = areaSo.Size.y;

        cells = new Grid2D[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                cells[x, y] = new Grid2D(new Vector2Int(x, y));

        float halfX = (width - 1) * 0.5f;
        float halfY = (height - 1) * 0.5f;

        OriginWorld = new Vector2(-halfX, -halfY);

        SpawnFixedEntities();
    }
    public void SpawnFixedEntities()
    {
        for (int i = 0; i < areaSo.FixedSpawnList.Count; i++)
        {
            FixedSpawnEntry entry = areaSo.FixedSpawnList[i];

            Vector2Int cellPos = entry.cellPos;
            BaseEntity entity = Managers.Object.Spawn<BaseEntity>(this, cellPos, entry.entitySo.Id);

            cells[cellPos.x, cellPos.y].Push(entity);
        }
    }
    public void ExploreArea()
    {
        for (int i = 0; i < areaSo.ExploreSpawnList.Count; i++)
        {
            ExploreSpawnEntry entry = areaSo.ExploreSpawnList[i];

            WeightOption weightOption = entry.countWeights.Pick();

            int spawnCount = weightOption.Value;
            if (spawnCount <= 0)
                continue;

            for (int j = 0; j < spawnCount; j++)
            {
                // 최소 수정: 일단 (0,0)에 생성 (추후 빈 칸 랜덤 배치로 교체)
                Vector2Int cellPos = new Vector2Int(0, 0);

                BaseEntity baseEntity = Managers.Object.Spawn<BaseEntity>(this, cellPos, entry.entitySo.Id);
                extraEntities.Add(baseEntity);

                cells[cellPos.x, cellPos.y].Push(baseEntity);
            }
        }
    }
    public void ExitArea()
    {
        for (int i = extraEntities.Count - 1; i >= 0; i--)
        {
            BaseEntity entity = extraEntities[i];
            Destroy(entity.gameObject);
        }

        extraEntities.Clear();
        gameObject.SetActive(false);
    }

    public bool TryWorldToNearestCell(Vector2 worldPos, out Vector2Int cellPos)
    {
        Vector2 local = worldPos - OriginWorld;

        int x = Mathf.FloorToInt(local.x + 0.5f);
        int y = Mathf.FloorToInt(local.y + 0.5f);

        if (x < 0 || y < 0 || x >= Size.x || y >= Size.y)
        {
            cellPos = default;
            return false;
        }

        cellPos = new Vector2Int(x, y);
        return true;
    }
    public bool TryGetEntitiesAtCell(Vector2Int cellPos, out IReadOnlyList<BaseEntity> list)
    {
        if (cellPos.x < 0 || cellPos.y < 0 || cellPos.x >= Size.x || cellPos.y >= Size.y)
        {
            list = null;
            return false;
        }

        list = cells[cellPos.x, cellPos.y].Entities;
        return true;
    }
}