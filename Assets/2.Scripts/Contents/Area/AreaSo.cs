using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

[Serializable]
public class FixedSpawnEntry
{
    public BaseEntitySo entitySo;
    public Vector3 localPos;
}

[Serializable]
public class ExploreSpawnEntry
{
    public BaseEntitySo entitySo;
    public int spawnCount;
    public float spawnPercent;
}


[CreateAssetMenu(menuName = "Game/Area/Area", fileName = "Area")]
public class AreaSo : ScriptableObject
{
    public string Id { get; private set; } = string.Empty;

    [SerializeField] private Area areaPrefab;
    [SerializeField] private LocalizedString areaName;
    [SerializeField] private Sprite backgroundSprite;
    [SerializeField] private List<LocalizedString> backgroundText;
    [SerializeField] private List<FixedSpawnEntry> fixedSpawnEntry;
    [SerializeField] private List<ExploreSpawnEntry> exploreSpawnEntry;
    [SerializeField] private bool resetArea = true;
    public Area AreaPrefab => areaPrefab;
    public LocalizedString AreaName => areaName;
    public List<LocalizedString> BackgroundText => backgroundText;
    public Sprite BackgroundSprite => backgroundSprite;
    public List<FixedSpawnEntry> FixedSpawnEntry => fixedSpawnEntry;
    public List<ExploreSpawnEntry> ExploreSpawnEntry => exploreSpawnEntry;
    public bool ResetArea => resetArea;

    public void SetId(string id)
    {
        if (string.IsNullOrEmpty(Id))
            Id = id;
        else
            Debug.LogError($"Id 중복 할당:{Id} -> {id}");
    }
} 