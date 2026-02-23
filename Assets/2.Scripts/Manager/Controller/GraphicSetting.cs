using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GraphicsSetting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume globalVolume;      // Global Volume

    [SerializeField] private float exposureMin = -2f;
    [SerializeField] private float exposureMax = 2f;

    private ColorAdjustments colorAdjustments; // cached

    public Vector2Int Resolution { get; private set; }
    public bool IsFullscreen { get; private set; }
    public bool GridOn { get; private set; }
    public float EffectIntensity { get; private set; }
    public float Brightness { get; private set; } // 0~1

    // -------------------- Setters --------------------

    public void SetBrightness(float value01)
    {
        Brightness = Mathf.Clamp01(value01);

        float exposure = Mathf.Lerp(exposureMin, exposureMax, Brightness);
        colorAdjustments.postExposure.overrideState = true;
        colorAdjustments.postExposure.value = exposure;
    }

    public void SetGridOn(bool on)
    {
        GridOn = on;
    }

    public void SetEffectIntensity(float value)
    {
        EffectIntensity = value;
    }

    public void SetWindowMode(bool isOn)
    {
        IsFullscreen = isOn;
        ApplyScreen();
    }

    public void SetResolution(Vector2Int res)
    {
        Resolution = new Vector2Int(Mathf.Max(640, res.x), Mathf.Max(480, res.y));
        ApplyScreen();
    }

    private void ApplyScreen()
    {
        FullScreenMode mode = IsFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

        Screen.SetResolution(Resolution.x, Resolution.y, mode);
    }

    private void CacheVolumeOverrides()
    {
        colorAdjustments = null;

        if (globalVolume == null || globalVolume.profile == null)
            return;

        globalVolume.profile.TryGet(out colorAdjustments);
    }

    #region 저장 및 로드
    // 2) 저장값 로드 적용(파일 있을 때)
    public void Load(SettingData data)
    {
        CacheVolumeOverrides();

        SetBrightness(data.brightness);
        SetGridOn(data.gridOn);
        SetEffectIntensity(data.effectIntensity);

        IsFullscreen = data.isFullscreen;
        Resolution = new Vector2Int(data.resolutionW, data.resolutionH);

        ApplyScreen();
    }

    // 3) 저장 전 DTO에 현재값 반영
    public void SaveTo(SettingData data)
    {
        data.brightness = Brightness;
        data.gridOn = GridOn;
        data.effectIntensity = EffectIntensity;
        data.isFullscreen = IsFullscreen;
        data.resolutionW = Resolution.x;
        data.resolutionH = Resolution.y;
    }
    #endregion
}