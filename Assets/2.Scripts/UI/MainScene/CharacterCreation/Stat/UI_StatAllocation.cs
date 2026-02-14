using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    private MainStat mainStatModel;

    public MainStatType MainStatType { get => mainStatType; }
    public void Init()
    {
        mainStatModel = new MainStat(mainStatType, 0);

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
        if (mainStatModel == null)
            return string.Empty;

        // 1) 소개 문구(LocalizedString)
        // 예: "{0} +1 당 증가량: ", "Per +1 {0}:" 같은 템플릿을 가정, 
        statIntroduceLocalize.Arguments = new object[]
        {
            mainStatModel.Name
        };

        StringBuilder sb = new StringBuilder(128);
        sb.AppendLine(statIntroduceLocalize.GetLocalizedString());

        // 3) 서브스탯 증가량 출력
        foreach (KeyValuePair<SubStatType, float> kv in mainStatModel.SubStatPerPointDic)
        {
            SubStat sub = new SubStat(kv.Key, kv.Value);

            sb.Append("- ");
            sb.Append(sub.Name);
            sb.Append(": ");
            sb.AppendLine(sub.DisplayValue);
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