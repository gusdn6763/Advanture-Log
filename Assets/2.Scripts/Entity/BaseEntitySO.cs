using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Game/BaseEntity", fileName = "BaseEntity")]
public class BaseEntitySo : ScriptableObject
{
    [SerializeField] private int id;

    [Header("메뉴")][SerializeField] private List<ActionMenuSo> actionMenus = new List<ActionMenuSo>();
    [Header("이름")] [SerializeField] protected LocalizedString objectName;  //플레이어는 objectName을 사용하지 않음
    [Header("툴팁-설명")] [SerializeField] private LocalizedString description;
    [Header("이미지")][SerializeField] private AssetReferenceSprite spriteRef;
    [Header("프리팹")][SerializeField] private AssetReferenceGameObject entityPrefabRef;

    public virtual bool UsesLocalizedName { get => true; } // 기본: 다국어 사용

    public int Id => id;
    public List<ActionMenuSo> ActionMenus { get => actionMenus; }
    public LocalizedString ObjectName { get => objectName; }
    public LocalizedString Description { get => description; }
    public AssetReferenceSprite Sprite { get => spriteRef; }
    public AssetReferenceGameObject EntityPrefab { get => entityPrefabRef; }
}