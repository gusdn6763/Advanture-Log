using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Title : MonoBehaviour
{
    [SerializeField] private UI_CharacterCreation characterCreation;
    [SerializeField] private GameObject loadUi;
    [SerializeField] private GameObject diaryUi;

    [Header("메뉴 버튼")]
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button gameContinueButton;
    [SerializeField] private Button diaryButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button quitButton;

    public void Init()
    {
        characterCreation.Init();

        BindButton(gameStartButton, StartGame);
        BindButton(gameContinueButton, LoadGame);
        BindButton(diaryButton, OpenDiary);
        BindButton(settingButton, OpenSetting);
        BindButton(quitButton, GameEnd);

        characterCreation.gameObject.SetActive(false);
        loadUi.SetActive(false);
        diaryUi.SetActive(false);
    }

    private static void BindButton(Button button, Action action)
    {
        if (button == null)
            return;

        button.SetClick(() => action?.Invoke());
    }


    public void StartGame()
    {
        characterCreation.gameObject.SetActive(true);
    }

    public void LoadGame()
    {
    }

    public void OpenDiary()
    {
        diaryUi.SetActive(true);
    }

    public void OpenSetting()
    {
        Managers.UI.OpenSettingUI();
    }

    public void GameEnd()
    {
        Application.Quit();
    }
}