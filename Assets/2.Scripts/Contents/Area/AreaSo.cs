using System;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("배경 텍스트")]
    [SerializeField] private List<LocalizedString> backgroundText = new List<LocalizedString>();

    [Header("고정 스폰")]
    [SerializeField] private List<FixedSpawnEntry> fixedSpawnEntry = new List<FixedSpawnEntry>();

    [Header("탐색 스폰")]
    [SerializeField] private List<ExploreSpawnEntry> exploreSpawnEntry = new List<ExploreSpawnEntry>();

    [Header("프리팹")]
    [SerializeField] private Area areaPrefab;

    [Header("지역 이름")]
    [SerializeField] private LocalizedString areaName;

    [Header("배경 스프라이트")]
    [SerializeField] private Sprite backgroundSprite;

    [Header("지역 리셋 여부")]
    [SerializeField] private bool resetArea;

    public IReadOnlyList<LocalizedString> BackgroundText => backgroundText;
    public IReadOnlyList<FixedSpawnEntry> FixedSpawnEntry => fixedSpawnEntry;
    public IReadOnlyList<ExploreSpawnEntry> ExploreSpawnEntry => exploreSpawnEntry;
    public Area AreaPrefab => areaPrefab;
    public LocalizedString AreaName => areaName;
    public Sprite BackgroundSprite => backgroundSprite;
    public bool ResetArea => resetArea;

    public void SetId(string id)
    {
        if (string.IsNullOrEmpty(Id))
            Id = id;
        else if (Id != id)
            Debug.LogError($"Id 중복 할당: {Id} -> {id}");
    }
}