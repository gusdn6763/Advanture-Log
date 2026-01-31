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
    [SerializeField] private string areaId;
    [SerializeField] private LocalizedString areaName;
    [SerializeField] private AssetReferenceSprite backgroundSpriteRef;
    [SerializeField] private List<LocalizedString> backgroundText;
    [SerializeField] private List<FixedSpawnEntry> fixedSpawnEntry;
    [SerializeField] private List<ExploreSpawnEntry> exploreSpawnEntry;

    public string AreaId => areaId;
    public LocalizedString AreaName => areaName;
    public List<LocalizedString> BackgroundText => backgroundText;
    public AssetReferenceSprite BackgroundSpriteRef => backgroundSpriteRef;
    public List<FixedSpawnEntry> FixedSpawnEntry => fixedSpawnEntry;
    public List<ExploreSpawnEntry> ExploreSpawnEntry => exploreSpawnEntry;
} 