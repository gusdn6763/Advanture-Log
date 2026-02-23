using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UI_JobSelection : MonoBehaviour, ICharacterCreationSection
{
    public event Action<PlayerSo> OnJobSelected;

    [SerializeField] private UI_JobButton jobButtonPrefab;
    [SerializeField] private Transform jobButtonTransform;

    [SerializeField] private UI_JobInfo jobInfo;

    [SerializeField] private Color selectedColor = Color.gray;
    [SerializeField] private Color normalColor = Color.white;

    private List<UI_JobButton> buttons = new List<UI_JobButton>();
    private UI_JobButton selectedButton;

    public void Init()
    {
        IEnumerable<PlayerSo> PlayerDatas = Managers.Data.GetPlayers();
        foreach (PlayerSo so in PlayerDatas)
        {
            UI_JobButton button = Instantiate(jobButtonPrefab, jobButtonTransform);
            button.Init(so);

            button.OnHovered += jobInfo.ShowJobInfo;
            button.OnClicked += SelectJobButton;

            buttons.Add(button);
        }

        jobInfo.Init();

        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(Locale locale)
    {
        foreach (UI_JobButton button in buttons)
            button.UpdateName();
    }

    private void SelectJobButton(UI_JobButton clicked)
    {
        if (selectedButton != null)
            selectedButton.SetColor(normalColor);

        selectedButton = clicked;
        selectedButton.SetColor(selectedColor);

        OnJobSelected?.Invoke(clicked.ButtonPlayerSo);
    }

    #region ICharacterCreationSection
    public void Refresh()
    {
        foreach (UI_JobButton button in buttons)
            button.SetColor(normalColor);

        selectedButton = null;
        jobInfo.Refresh();
    }

    public bool IsValid()
    {
        return selectedButton != null;
    }

    public void Apply(GameStartData data)
    {
        if (selectedButton != null)
            data.jobId = selectedButton.ButtonPlayerSo.Id;
    }
    #endregion
}