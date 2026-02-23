using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Graphics : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle screenModeToggle;
    [SerializeField] private Toggle gridToggle;
    [SerializeField] private Slider effectSlider;
    [SerializeField] private Slider brightnessSilder;

    private List<Resolution> resolutions = new List<Resolution>();

    private GraphicsSetting graphics;

    public void Init()
    {
        graphics = Managers.Setting.GraphicsSetting;
        Managers.Setting.OnReset += Refresh;

        resolutions.AddRange(Screen.resolutions);
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();

            option.text = item.width + "x" + item.height;
            resolutionDropdown.options.Add(option);
        }

        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(OnResolutionIndex);

        screenModeToggle.onValueChanged.AddListener(OnScreenMode);
        gridToggle.onValueChanged.AddListener(OngridToggle);

        effectSlider.onValueChanged.AddListener(OnEffectIntensity);
        brightnessSilder.onValueChanged.AddListener(Onbrightness);
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void OnDestroy()
    {
        Managers.Setting.OnReset -= Refresh;
    }
    private void Refresh()
    {
        if (graphics == null)
            return;

        resolutionDropdown.SetValueWithoutNotify(FindClosestResolutionIndex(graphics.Resolution));

        gridToggle.SetIsOnWithoutNotify(graphics.GridOn);
        screenModeToggle.SetIsOnWithoutNotify(graphics.IsFullscreen);

        brightnessSilder.SetValueWithoutNotify(graphics.Brightness);
        effectSlider.SetValueWithoutNotify(graphics.EffectIntensity);
    }

    private int FindClosestResolutionIndex(Vector2Int current)
    {
        int best = 0;
        int bestScore = int.MaxValue;

        for (int i = 0; i < resolutions.Count; i++)
        {
            int score = Mathf.Abs(resolutions[i].width - current.x) + Mathf.Abs(resolutions[i].height - current.y);

            if (score < bestScore)
            {
                bestScore = score; 
                best = i;
            }
        }

        return best;
    }

    private void OnResolutionIndex(int idx)
    {
        graphics.SetResolution(new Vector2Int(resolutions[idx].width, resolutions[idx].height));
    }
    private void OnScreenMode(bool check)
    {
        graphics.SetWindowMode(check);
    }
    private void OngridToggle(bool on)
    {
        graphics.SetGridOn(on);
    }
    private void OnEffectIntensity(float value)
    {
        graphics.SetEffectIntensity(value);
    }
    private void Onbrightness(float value)
    {
        graphics.SetBrightness(value);
    }
}