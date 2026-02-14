using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Area : MonoBehaviour
{
    private AreaSo areaSo;

    public Sprite BackgroundSprite { get; private set; }
    public List<BaseEntity> FixedEntities { get; private set; } = new List<BaseEntity>();
    public List<BaseEntity> ExtraEntities { get; private set; } = new List<BaseEntity>();
    public void SetInfo(AreaSo data)
    {
        areaSo = data;
    }

    public void SpawnFixedEntities()
    {
        for (int i = 0; i < areaSo.FixedSpawnEntry.Count; i++)
        {
            FixedSpawnEntry entry = areaSo.FixedSpawnEntry[i];

            BaseEntity entity = Managers.Object.Spawn<BaseEntity>(entry.localPos, entry.entitySo.Id);

            FixedEntities.Add(entity);
        }
    }

    public void SpawnEntities()
    {
    }

    public void ExitArea()
    {
        for(int i = ExtraEntities.Count; i > 0; i--)
        {
            BaseEntity entity = ExtraEntities[i];

            Destroy(entity.gameObject);
            entity = null;
        }

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        BackgroundSprite = null;
        FixedEntities.Clear();
        ExtraEntities.Clear();
    }
}