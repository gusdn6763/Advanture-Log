using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class UI_StatAllocation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<string> OnHovered;
    public event Action<MainStatType, int> OnClicked;

    [SerializeField] private LocalizedString statIntroduceLocalize;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button plusButton;

    [SerializeField] private MainStatType mainStatType;

    public MainStatType MainStatType { get => mainStatType; }
    public void Init()
    {
        minusButton.onClick.RemoveAllListeners();
        plusButton.onClick.RemoveAllListeners();

        minusButton.onClick.AddListener(() => OnClick(-1));
        plusButton.onClick.AddListener(() => OnClick(+1));
    }

    public void OnClick(int delta)
    {
        OnClicked?.Invoke(mainStatType, delta);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string str = statIntroduceLocalize.GetLocalizedString();

        OnHovered?.Invoke(str);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnHovered?.Invoke(string.Empty);
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