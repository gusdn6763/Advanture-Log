using UnityEngine;
using UnityEngine.Localization;

public class SkillSo : ScriptableObject
{
    public string Id { get; private set; } = string.Empty;

    [SerializeField] private LocalizedString skillNameLocalized;
    [SerializeField] private LocalizedString skillDescriptionLocalized;
    [SerializeField] private Sprite skillImage;
    [SerializeField] private float baseDagame;
    [SerializeField] private float damageMultiplier;

    public LocalizedString SkillNameLocalized { get => skillNameLocalized; }
    public LocalizedString SkillDescriptionLocalized { get => skillDescriptionLocalized; }
    public Sprite SkillImage { get => skillImage; }
    public float BaseDagame { get => baseDagame; }
    public float DamageMultiplier { get => damageMultiplier; }
}