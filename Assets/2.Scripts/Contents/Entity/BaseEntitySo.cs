using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public abstract class BaseEntitySo : ScriptableObject
{
    public string Id { get; private set; } = string.Empty;

    [Header("상호작용 가능 리스트")]
    [SerializeField] private List<ActionMenuSo> actionMenus = new List<ActionMenuSo>();

    [Header("이름")]
    [SerializeField] protected LocalizedString objectNameLocalized;

    [Header("설명")]
    [SerializeField] private LocalizedString descriptionLocalized;

    [Header("이미지")]
    [SerializeField] private Sprite entityImage;

    [Header("프리팹")]
    [SerializeField] private BaseEntity entityPrefab;

    [Header("블럭 여부")]
    [SerializeField] private bool isBlock;

    public IReadOnlyList<ActionMenuSo> ActionMenus { get => actionMenus; }
    public LocalizedString ObjectNameLocalized { get => objectNameLocalized; }
    public LocalizedString DescriptionLocalized { get => descriptionLocalized; }
    public Sprite EntityImage { get => entityImage; }
    public BaseEntity EntityPrefab { get => entityPrefab; }
    public bool IsBlock { get => isBlock; }

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