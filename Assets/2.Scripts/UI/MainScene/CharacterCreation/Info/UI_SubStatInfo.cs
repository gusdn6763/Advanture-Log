using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Localization;

public class UI_SubStatInfo : MonoBehaviour, ITooltipProvider
{
    [SerializeField] private LocalizedString statIntroduceLocalize;
    [SerializeField] private MainStatType mainStatType;
    private MainStat mainStatModel;

    private void Awake()
    {
        mainStatModel = new MainStat(mainStatType, 1);
    }

    public bool TryGetTooltipData(TooltipData tooltipData)
    {
        if (mainStatModel == null)
            return false;

        // 1) 소개 문구(LocalizedString)
        // 예: "{0} +1 당 증가량: ", "Per +1 {0}:" 같은 템플릿을 가정, 
        statIntroduceLocalize.Arguments = new object[]
        {
            mainStatModel.Name
        };

        // 3) 서브스탯 증가량 출력
        StringBuilder sb = new StringBuilder(128);
        foreach (KeyValuePair<SubStatType, float> kv in mainStatModel.SubStatPerPointDic)
        {
            SubStat sub = new SubStat(kv.Key, kv.Value);

            sb.Append("- ");
            sb.Append(sub.Name);
            sb.Append(": ");
            sb.AppendLine(sub.DisplayValue);
        }

        tooltipData.tooltipWidthType = TooltipWidthType.Auto;
        tooltipData.textType = TooltipTextAlignType.Left;
        tooltipData.positionType = TooltipPositionType.Right;
        tooltipData.header = statIntroduceLocalize.GetLocalizedString();
        tooltipData.body = sb.ToString();
        
        return true;
    }
}