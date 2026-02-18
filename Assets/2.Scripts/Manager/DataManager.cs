using AYellowpaper.SerializedCollections;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataManager : MonoBehaviour
{
    [Header("상호작용 오브젝트")]
    [SerializeField] private SerializedDictionary<string, BaseEntitySo> entites = new SerializedDictionary<string, BaseEntitySo>();
    [SerializeField] private SerializedDictionary<string, ItemEntitySo> items = new SerializedDictionary<string, ItemEntitySo>();
    [SerializeField] private SerializedDictionary<string, MonsterSo> monsters = new SerializedDictionary<string, MonsterSo>();
    [SerializeField] private SerializedDictionary<string, PlayerSo> players = new SerializedDictionary<string, PlayerSo>();
    [SerializeField] private SerializedDictionary<string, AreaSo> areas = new SerializedDictionary<string, AreaSo>();

    private Dictionary<string, BaseEntitySo> enetityDic = new Dictionary<string, BaseEntitySo>();

    public IEnumerable<PlayerSo> PlayerDatas { get => players.Values; }

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
    public bool TryGetArea<T>(string id, out T so) where T : AreaSo
    {
        if (areas.TryGetValue(id, out AreaSo baseSo))
        {
            so = baseSo as T;
            return true;
        }
        so = null;
        return false;
    }

    #region 초기화
    public void Init()
    {
        enetityDic.Clear();

        MergeIntoEntityDic(entites);
        MergeIntoEntityDic(items);
        MergeIntoEntityDic(monsters);
        MergeIntoEntityDic(players);

        foreach (KeyValuePair<string, AreaSo> area in areas)
            area.Value.SetId(area.Key);
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

    #region 플레이어 규칙
    [Header("플레이어 관련 데이터")]
    [SerializeField] private PlayerRuleSo playerRuleSo;

    public PlayerRuleSo PlayerRule { get => playerRuleSo; }

    #endregion

    #region 스탯 규칙
    [Header("스탯 규칙 관련 데이터")]
    [SerializeField] private StatRuleSo statRuleSo;

    public StatRuleSo StatRule { get => statRuleSo; }
    #endregion

    #region 아이템 규칙
    [Header("아이템 규칙 관련 데이터")]
    [SerializeField] private ItemRuleSo itemRuleSo;
    public ItemRuleSo ItemRule => itemRuleSo;
    #endregion
}