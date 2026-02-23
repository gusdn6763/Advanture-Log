using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatAllocation : MonoBehaviour
{
    public event Action<MainStatType, int> OnClicked;

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

        minusButton.SetClick(() => OnClick(-1));
        plusButton.SetClick(() => OnClick(+1));
    }

    public void OnClick(int delta)
    {
        OnClicked?.Invoke(mainStatType, delta);
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