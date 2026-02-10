using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_JobSelection : MonoBehaviour, ICharacterCreationSection
{
    public event Action<string> OnJobSelected;

    [SerializeField] protected UI_JobButton jobButtonPrefab;
    [SerializeField] private TextMeshProUGUI jobIntroduceText;
    [SerializeField] private Transform content;

    private List<UI_JobButton> jobButtons = new List<UI_JobButton>();
    private string selectedPlayerId;

    public void Init()
    {
        foreach (PlayerSo playerSo in Managers.Data.PlayerDatas)
        {
            UI_JobButton jobButton = Instantiate(jobButtonPrefab, content);
            jobButton.Init(playerSo);

            jobButton.OnHovered += UpdateJobExplan;
            jobButton.OnClicked += SelectJobButton;

            jobButtons.Add(jobButton);
        }
    }

    private void UpdateJobExplan(string str)
    {
        jobIntroduceText.text = str;
    }
    private void SelectJobButton(string jobId)
    {
        selectedPlayerId = jobId;
        OnJobSelected?.Invoke(selectedPlayerId);
    }

    #region ICharacterCreationSection
    public void Refresh()
    {
        selectedPlayerId = string.Empty;
        UpdateJobExplan(string.Empty);
    }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(selectedPlayerId);
    }

    public void Apply(GameStartData data)
    {
        data.jobId = selectedPlayerId;
    }
    #endregion
}