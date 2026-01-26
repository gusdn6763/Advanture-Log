using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

public class SpawnEntry
{
    public BaseEntitySO entitySo;
    public Vector3 localPos;
    public string instanceId;
}


[CreateAssetMenu(menuName = "Game/Area", fileName = "Area")]
public class AreaSo : ScriptableObject
{
    [SerializeField] private string areaId;
    [SerializeField] private LocalizedString areaName;
    [SerializeField] private List<LocalizedString> backgroundText;
    [SerializeField] private AssetReferenceSprite backgroundSpriteRef;
    [SerializeField] private List<AssetReferenceGameObject> createObjectRefs;

    public string AreaId => areaId;
    public LocalizedString AreaName => areaName;
    public List<LocalizedString> BackgroundText => backgroundText;
    public AssetReferenceSprite BackgroundSpriteRef => backgroundSpriteRef;
    public List<AssetReferenceGameObject> CreateObjectRefs => createObjectRefs;
} 