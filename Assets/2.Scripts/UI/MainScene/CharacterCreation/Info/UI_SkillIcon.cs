using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillIcon : MonoBehaviour, ITooltipProvider
{
    [SerializeField] private Image skillImage;
    [SerializeField] private TextMeshProUGUI skillNameText;

    private string skillDescription;

    public void SetInfo(SkillSo skillSo)
    {
        skillNameText.text = skillSo.SkillNameLocalized.GetLocalizedString();
        skillImage.sprite = skillSo.SkillImage;
        skillDescription = skillSo.SkillDescriptionLocalized.GetLocalizedString();
    }

    public string GetTooltipContent()
    {
        return skillDescription;
    }
}