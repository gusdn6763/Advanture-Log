using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gameplay : MonoBehaviour
{
    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown languageDropdown;  
    [SerializeField] private TMP_InputField autoSavePeriodInputField;

    private GameplaySetting gameplay;

    public void Init()
    {
        gameplay = Managers.Setting.GameplaySetting;
        Managers.Setting.OnReset += Refresh;

        languageDropdown.onValueChanged.AddListener(ChangeLanguage);
        autoSavePeriodInputField.onValueChanged.AddListener(ChangeAutoSavePeriod);
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
        if (gameplay == null)
            return;

        languageDropdown.SetValueWithoutNotify(gameplay.LocaleIndex);

        autoSavePeriodInputField.text = gameplay.AutoSavePeriod.ToString();
    }

    private void ChangeLanguage(int idx)
    {
        gameplay.SetLocaleIndex(idx);
    }
    private void ChangeAutoSavePeriod(string str)
    {
        gameplay.SetAutoSavePeriod(int.Parse(str));
    }
}