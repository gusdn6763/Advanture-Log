using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public abstract class BaseEntitySo : ScriptableObject
{
    public string Id { get; private set; } = string.Empty;

    [Header("메뉴")][SerializeField] private List<ActionMenuSo> actionMenus = new List<ActionMenuSo>();
    [Header("이름")] [SerializeField] protected LocalizedString objectNameLocalized;  //플레이어는 objectName을 사용하지 않음
    [Header("툴팁-설명")] [SerializeField] private LocalizedString descriptionLocalized;
    [Header("이미지")][SerializeField] private Sprite entityImage;
    [Header("프리팹")][SerializeField] private BaseEntity entityPrefab;

    public virtual bool UsesLocalizedName { get => true; } // 기본: 다국어 사용
    public List<ActionMenuSo> ActionMenus { get => actionMenus; }
    public LocalizedString ObjectNameLocalized { get => objectNameLocalized; }
    public LocalizedString DescriptionLocalized { get => descriptionLocalized; }
    public Sprite EntityImage { get => entityImage; }
    public BaseEntity EntityPrefab { get => entityPrefab; }

    public void SetId(string id)
    {
        if (string.IsNullOrEmpty(Id))
        {
            Id = id;
            return;
        }

        if (Id == id)
            return;

        Debug.LogError($"Id 재 할당:{Id} -> {id}");
    }
}