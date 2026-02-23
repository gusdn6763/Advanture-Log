using System.Collections.Generic;
using UnityEngine;


public class Area : MonoBehaviour
{
    private AreaSo areaSo;

    public List<BaseEntity> fixedEntities = new List<BaseEntity>();
    public List<BaseEntity> extraEntities = new List<BaseEntity>();

    public Sprite BackgroundSprite { get; private set; }
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

            fixedEntities.Add(entity);
        }
    }

    public void SpawnEntities()
    {
    }

    public void ExitArea()
    {
        for(int i = extraEntities.Count; i > 0; i--)
        {
            BaseEntity entity = extraEntities[i];

            Destroy(entity.gameObject);
            entity = null;
        }

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        BackgroundSprite = null;
        fixedEntities.Clear();
        extraEntities.Clear();
    }
}