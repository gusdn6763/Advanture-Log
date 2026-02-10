using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

public abstract class BaseEntitySo : ScriptableObject
{
    public string Id { get; private set; } = string.Empty;
    public virtual ObjectType ObjectType { get; protected set; } = ObjectType.None;

    [Header("메뉴")][SerializeField] private List<ActionMenuSo> actionMenus = new List<ActionMenuSo>();
    [Header("이름")] [SerializeField] protected LocalizedString objectName;  //플레이어는 objectName을 사용하지 않음
    [Header("툴팁-설명")] [SerializeField] private LocalizedString description;
    [Header("이미지")][SerializeField] private Sprite entityImage;
    [Header("프리팹")][SerializeField] private GameObject entityPrefab;

    public virtual bool UsesLocalizedName { get => true; } // 기본: 다국어 사용
    public List<ActionMenuSo> ActionMenus { get => actionMenus; }
    public LocalizedString ObjectName { get => objectName; }
    public LocalizedString Description { get => description; }
    public Sprite EntityImage { get => entityImage; }
    public GameObject EntityPrefab { get => entityPrefab; }

    public void SetId(string id)
    {
        if (string.IsNullOrEmpty(Id))
            Id = id;
        else
            Debug.LogError($"Id 중복 할당:{Id} -> {id}");
    }
}