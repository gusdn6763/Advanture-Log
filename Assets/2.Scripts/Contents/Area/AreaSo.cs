using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public class FixedSpawnEntry
{
    public BaseEntitySo entitySo;
    public Vector2Int cellPos;
}

[Serializable]
public class ExploreSpawnEntry
{
    public BaseEntitySo entitySo;

    [Header("스폰 갯수 가중치")]
    public WeightedTable countWeights = new WeightedTable();
}

[CreateAssetMenu(menuName = "Game/Area/Area", fileName = "Area")]
public class AreaSo : ScriptableObject
{
    public string Id { get; private set; } = string.Empty;

    [Header("배경 텍스트")]
    [SerializeField] private List<LocalizedString> backgroundText = new List<LocalizedString>();

    [Header("고정 스폰")]
    [SerializeField] private List<FixedSpawnEntry> fixedSpawnList = new List<FixedSpawnEntry>();

    [Header("탐색 스폰")]
    [SerializeField] private List<ExploreSpawnEntry> exploreSpawnList = new List<ExploreSpawnEntry>();

    [Header("프리팹")]
    [SerializeField] private Area areaPrefab;

    [Header("지역 이름")]
    [SerializeField] private LocalizedString areaName;

    [Header("배경 스프라이트")]
    [SerializeField] private Sprite backgroundSprite;

    [Header("지역 리셋 여부")]
    [SerializeField] private bool resetArea;

    [Header("크기")]
    [SerializeField] private Vector2Int size;

    public IReadOnlyList<LocalizedString> BackgroundText => backgroundText;
    public IReadOnlyList<FixedSpawnEntry> FixedSpawnList => fixedSpawnList;
    public IReadOnlyList<ExploreSpawnEntry> ExploreSpawnList => exploreSpawnList;
    public Area AreaPrefab => areaPrefab;
    public LocalizedString AreaName => areaName;
    public Sprite BackgroundSprite => backgroundSprite;
    public bool ResetArea => resetArea;
    public Vector2Int Size => size;
    public void SetId(string id)
    {
        if (string.IsNullOrEmpty(Id))
            Id = id;
        else if (Id != id)
            Debug.LogError($"Id 중복 할당: {Id} -> {id}");
    }
}