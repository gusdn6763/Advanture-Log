using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SliderValueText : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (slider != null)
            ShowSliderValue(slider.value);
        else if (text != null)
            text.text = string.Empty;
    }

    public void ShowSliderValue(float value)
    {
        if (text != null)
            text.text = (value * 100f).ToString("F0") + "%";
    }
}