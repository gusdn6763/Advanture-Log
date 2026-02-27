using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RangeDialog : UI_PopupBase
{
    [SerializeField] private Slider rangeSlider;
    [SerializeField] private TextMeshProUGUI currentValue;
    [SerializeField] private Button okButton;

    private Action<int> onOk;

    public void SetInfo(int min, int max, int defaultValue, Action<int> onOk)
    {
        this.onOk = onOk;

        // 값 세팅
        if (rangeSlider != null)
        {
            rangeSlider.wholeNumbers = true;
            rangeSlider.minValue = Mathf.Max(0, min);
            rangeSlider.maxValue = Mathf.Max(rangeSlider.minValue, max);
            rangeSlider.value = Mathf.Clamp(defaultValue, (int)rangeSlider.minValue, (int)rangeSlider.maxValue);
            rangeSlider.onValueChanged.AddListener(SliderChange);
        }

        // 버튼 리스너 재바인딩
        if (okButton != null)
        {
            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(OnClickOk);
        }
    }

    public void SliderChange(float value)
    {

    }

    public void Close()
    {
        gameObject.SetActive(false);

        if (okButton != null) 
            okButton.onClick.RemoveAllListeners();

        onOk = null;
    }

    private void OnClickOk()
    {
        onOk?.Invoke((int)rangeSlider.value);
        Close();
    }
}