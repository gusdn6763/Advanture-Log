using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region 상호작용 오브젝트
    [Header("상호작용 오브젝트")]
    [SerializeField] private SerializedDictionary<string, ItemEntitySo> items = new SerializedDictionary<string, ItemEntitySo>();
    [SerializeField] private SerializedDictionary<string, MonsterSo> monsters = new SerializedDictionary<string, MonsterSo>();
    [SerializeField] private SerializedDictionary<string, PlayerSo> players = new SerializedDictionary<string, PlayerSo>();

    private Dictionary<string, BaseEntitySo> enetityDic = new Dictionary<string, BaseEntitySo>();
    public IEnumerable<PlayerSo> GetPlayers() => players.Values;

    public bool TryGetEntity<T>(string id, out T so) where T : BaseEntitySo
    {
        if (enetityDic.TryGetValue(id, out BaseEntitySo baseSo))
        {
            so = baseSo as T;
            return true;
        }
        so = null;
        return false;
    }
    #endregion

    #region 초기화
    public void Init()
    {
        enetityDic.Clear();

        MergeIntoEntityDic(items);
        MergeIntoEntityDic(monsters);
        MergeIntoEntityDic(players);
    }
    private void MergeIntoEntityDic<T>(SerializedDictionary<string, T> source) where T : BaseEntitySo
    {
        foreach (KeyValuePair<string, T> kv in source)
        {
            string key = kv.Key;
            BaseEntitySo value = kv.Value;

            // ID 동기화
            value.SetId(key);

            // 키 중복 체크
            if (enetityDic.ContainsKey(key))
            {
                Debug.LogError($"[DataManager] 키 중복: '{key}'");
                continue; 
            }

            enetityDic.Add(key, value);
        }
    }
    #endregion

    [field: Header("스탯 규칙 관련 데이터")]
    [field:SerializeField] public StatRuleSo StatRule { get; private set; }

    [field: Header("아이템 규칙 관련 데이터")]
    [field: SerializeField] public ItemRuleSo ItemRule { get; private set; }
}