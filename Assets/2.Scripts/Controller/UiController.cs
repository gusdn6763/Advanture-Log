using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [Header("배경 화면")]
    [SerializeField] private Image backgroundImage;

    [Header("메뉴바")]
    [SerializeField] private UI_ActionMenuOverlay actionMenuBar;

    [Header("UI")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject quest;

    public void Init(Player player)
    {
        Managers.UI.BindSceneUi(this);

        CreatePoolingLog();
        actionMenuBar.Init();

        //체력바 등을 설정
    }

    private void OnEnable()
    {
        Managers.Area.OnAreaChanged += ChangeBackground;
    }

    private void OnDisable()
    {
        Managers.Area.OnAreaChanged -= ChangeBackground;
    }

    public void OpenUi(UiType type, BaseEntity target)
    {
        switch (type)
        {
            case UiType.ActionMenu:
                actionMenuBar.Open(target);
                break;
            case UiType.Inventory:
                inventory.gameObject.SetActive(true);
                break;
            case UiType.Quest:
                quest.gameObject.SetActive(true);
                break;
        }
    }

    public void ChangeBackground(Area currentArea)
    {
        if (currentArea)
            backgroundImage.sprite = currentArea.BackgroundSprite;
        else
            backgroundImage.sprite = null;
    }


    #region 로그창
    [Header("로그창")]
    [SerializeField] private UI_LogText logPrefab;
    [SerializeField] private Transform logTransform;

    [SerializeField] private int poolingLogCount = 10;

    private Queue<UI_LogText> poolLog = new Queue<UI_LogText>();
    private UI_LogText previousLog;

    private void CreatePoolingLog()
    {
        for (int i = 0; i < poolingLogCount; i++)
        {
            UI_LogText log = Instantiate(logPrefab, logTransform);
            log.Init();
            log.gameObject.SetActive(false);
            poolLog.Enqueue(log);
        }
    }

    public void CreateMessage(string message)
    {
        if (previousLog)
            previousLog.ColorChange();

        UI_LogText logText = poolLog.Dequeue();
        logText.SetText(message);
        logText.gameObject.SetActive(true);
        logText.transform.SetAsLastSibling();

        previousLog = logText;
        poolLog.Enqueue(logText);
    }
    #endregion
}