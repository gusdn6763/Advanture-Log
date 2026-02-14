using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class UI_StatAllocation : MonoBehaviour, ITooltipProvider
{
    public event Action<MainStatType, int> OnClicked;

    [SerializeField] private LocalizedString statIntroduceLocalize;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button plusButton;

    [SerializeField] private MainStatType mainStatType;

    public MainStatType MainStatType { get => mainStatType; }
    public void Init()
    {
        plusButton.interactable = false;
        minusButton.interactable = false;

        minusButton.onClick.RemoveAllListeners();
        plusButton.onClick.RemoveAllListeners();

        minusButton.onClick.AddListener(() => OnClick(-1));
        plusButton.onClick.AddListener(() => OnClick(+1));
    }

    public void OnClick(int delta)
    {
        OnClicked?.Invoke(mainStatType, delta);
    }

    public string GetTooltipContent()
    {
        StatRuleSo statRule = Managers.Data.StatRule;

        // 1) 메인 룰 가져오기
        if (!statRule.TryGet(mainStatType, out MainStatRule mainRule))
            return string.Empty;

        // 2) 소개 문구(LocalizedString)
        // 예: "Per +1 {0}:" 같은 템플릿을 가정
        statIntroduceLocalize.Arguments = new object[]
        {
            mainRule.StatName.GetLocalizedString()
        };

        StringBuilder sb = new StringBuilder(128);
        sb.AppendLine(statIntroduceLocalize.GetLocalizedString());

        // 3) 서브스탯 증가량 출력
        foreach (KeyValuePair<SubStatType, float> kv in mainRule.SubStatPerPointDic)
        {
            if (!statRule.SubStatDic.TryGetValue(kv.Key, out SubStatRule subRule))
                continue;

            string subName = subRule.StatName.GetLocalizedString();
            string subVal = StringUtil.FormatValueForDisplay(kv.Value, subRule.DisplayType);

            sb.Append("- ");
            sb.Append(subName);
            sb.Append(": ");
            sb.AppendLine(subVal);
        }

        return sb.ToString().TrimEnd(); // 마지막 개행 정리
    }

    #region 외부 호출
    public void SetValue(int value)
    {
        statText.text = value.ToString();
    }

    public void SetInteractableButtons(int remainingPoints, int currentValue, int minValue)
    {
        plusButton.interactable = remainingPoints > 0;
        minusButton.interactable = currentValue > minValue;
    }
    #endregion
}