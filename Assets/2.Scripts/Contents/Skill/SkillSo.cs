using UnityEngine;
using UnityEngine.Localization;

public class SkillSo : ScriptableObject
{
    public string Id { get; private set; } = string.Empty;

    [Header("이름")][SerializeField] private LocalizedString skillNameLocalized;
    [Header("설명")][SerializeField] private LocalizedString skillDescriptionLocalized;
    [Header("이미지")][SerializeField] private Sprite skillImage;
    [Header("공격력")][SerializeField] private float baseDagame;
    [Header("데미지 배율")][SerializeField] private float damageMultiplier;

    public LocalizedString SkillNameLocalized { get => skillNameLocalized; }
    public LocalizedString SkillDescriptionLocalized { get => skillDescriptionLocalized; }
    public Sprite SkillImage { get => skillImage; }
    public float BaseDagame { get => baseDagame; }
    public float DamageMultiplier { get => damageMultiplier; }
}