using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_SkillIcon : MonoBehaviour, ITooltipProvider
{
    private Image skillImage;

    private SkillSo SkillData { get; set; }

    public void Init(SkillSo skillSo)
    {
        skillImage = GetComponent<Image>();

        SkillData = skillSo;
        skillImage.sprite = skillSo.SkillImage;
    }

    #region ITooltipProvider

    public bool TryGetTooltipData(TooltipData tooltipData)
    {
        tooltipData = default;

        if (!SkillData)
            return false;

        tooltipData.tooltipWidthType = TooltipWidthType.Fixed;
        tooltipData.textType = TooltipTextAlignType.Left;
        tooltipData.positionType = TooltipPositionType.Right;
        tooltipData.header = SkillData.SkillNameLocalized.GetLocalizedString();
        tooltipData.body = SkillData.SkillDescriptionLocalized.GetLocalizedString();

        return true;
    }

    #endregion
}