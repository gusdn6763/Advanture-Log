using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
    [SerializeField] private Text descriptionText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button doingButton;
    [SerializeField] private Button completeButton;

    private Quest quest;
    public Quest MyQuest { get { return quest; } set { quest = value; } }

    public void Init()
    {
        
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    public void OnLocaleChanged(Locale locale)
    {

    }

    //퀘스트 수락
    public void AcceptQuest()
    {
        //if (Managers.Quest.AcceptQuest(this))
        {
            acceptButton.gameObject.SetActive(false);
            if (MyQuest.IsComplete)
                completeButton.gameObject.SetActive(true);
            else
                doingButton.gameObject.SetActive(true);
        }
        //else
            Managers.UI.ShowLog("제한 갯수 초과 or 등급 부족");
    }

    //퀘스트 포기
    public void GiveupQuest()
    {
        //Managers.Quest.GiveUpQuest(this);
        acceptButton.gameObject.SetActive(true);
        doingButton.gameObject.SetActive(false);
        completeButton.gameObject.SetActive(false);
    }

    //퀘스트 완료
    public void CompleteQuest()
    {
        //Player.Current.SetQuestReward();
        //Managers.Quest.CompleteQuest(this);
    }
}
