using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectManager : MonoBehaviour
{
    //HashSet = 중복 제거
    public HashSet<MobEntity> Monsters { get; } = new HashSet<MobEntity>();

    public Player CurrentPlayer { get; private set; }

    public T Spawn<T>(Area area, Vector2 cellPos, string id) where T : BaseEntity
    {
        if (!Managers.Data.TryGetEntity(id, out BaseEntitySo so))
        {
            Debug.LogError($"[Spawner] Entity id not found: '{id}'");
            return null;
        }

        BaseEntity entity = Instantiate(so.EntityPrefab, area.transform);
        entity.name = entity.name + "_" + id;
        entity.transform.position = area.OriginWorld + cellPos;
        entity.SetInfo(so);

        return entity as T;
    }

    public void Despawn<T>(T obj) where T : BaseEntity
    {

        Destroy(obj.gameObject);
    }

    public Player SpawnPlayer(Area area, GameStartData startData, Vector2 cellPos)
    {
        if (CurrentPlayer != null)
            Despawn(CurrentPlayer);

        Player player = Spawn<Player>(area, cellPos, startData.jobId);
        player.SetInfo(startData);

        CurrentPlayer = player;
        return player;
    }
}
