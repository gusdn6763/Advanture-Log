using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameStartData
{
    public bool load;
    public int saveSlot;

    public string playerName;
    public int str, agi, con, intl, per;
    public string jobId;
}

public class UI_CharacterCreation : MonoBehaviour
{
    [SerializeField] private UI_JobSelection jobSelection;
    [SerializeField] private UI_StatSelection statSelection;
    [SerializeField] private UI_PlayerNameSetting playerNameSetting;
    [SerializeField] private UI_GameModeSelection gameModeSelection;

    [Header("버튼")]
    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelButton;

    [Header("게임씬 로더")]
    [SerializeField] private GameSceneLoader gameSceneLoader;

    private List<ICharacterCreationSection> sections = new List<ICharacterCreationSection>();
    private GameStartData gameStartData = new GameStartData();

    public void Init()
    {
        sections.Add(jobSelection);
        sections.Add(statSelection);
        sections.Add(playerNameSetting);
        sections.Add(gameModeSelection);

        jobSelection.Init();
        statSelection.Init();

        //직업별 스탯 존재, 직업을 선택하기 전까지 스탯 조절 못하도록 수정
        jobSelection.OnJobSelected += statSelection.SetJob;

        okButton.onClick.AddListener(OkButton);
        cancelButton.onClick.AddListener(CloseButton);
    }

    private void OnEnable()
    {
        foreach (ICharacterCreationSection section in sections)
            section.Refresh();
    }


    private void OkButton()
    {
        bool isAllValid = true;
        foreach(ICharacterCreationSection section in sections)
        {
            if (section.IsValid())
                section.Apply(gameStartData);
            else
                isAllValid = false;
        }

        if (isAllValid) 
            gameSceneLoader.LoadScene(gameStartData);
    }
    private void CloseButton()
    {
        gameObject.SetActive(false);
    }
}