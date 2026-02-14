using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //HashSet = 중복 제거
    public HashSet<Monster> Monsters { get; } = new HashSet<Monster>();

    public Player CurrentPlayer { get; private set; }

    public T Spawn<T>(Vector2Int cellPos, string id) where T : BaseEntity
    {
        Vector2 spawnPos = Managers.Area.Cell2World(cellPos);
        return Spawn<T>(spawnPos, id);
    }

    public T Spawn<T>(Vector2 position, string id) where T : BaseEntity
    {
        if (!Managers.Data.TryGetEntity(id, out BaseEntitySo so) || so == null)
        {
            Debug.LogError($"[Spawner] Entity id not found: '{id}'");
            return null;
        }

        BaseEntity entity = Instantiate(so.EntityPrefab);
        entity.name = entity.name + "_" + id;
        entity.transform.position = position;
        entity.SetInfo(so);

        return entity as T;
    }

    public void Despawn<T>(T obj) where T : BaseEntity
    {

        Destroy(obj.gameObject);
    }

    public Player SpawnPlayer(PlayerData data)
    {
        if (CurrentPlayer != null)
            Despawn(CurrentPlayer);

        Player player = Spawn<Player>(Vector2.zero, data.playerType);
        if (player == null) 
            return null;

        player.SetUp(data);
        CurrentPlayer = player;
        return player;
    }
}
