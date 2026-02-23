using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TabItem
{
    public UI_TabPage tabPage;
    public GameObject page;
}

public class UI_Setting : MonoBehaviour
{
    [SerializeField] private List<TabItem> tabList = new List<TabItem>();

    [SerializeField] private Sprite activeButtonSprite;
    [SerializeField] private Sprite defaultButtonSprite;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button resetButton;

    private int currentIndex = -1;

    public void Init()
    {
        for (int i = 0; i < tabList.Count; i++)
        {
            UI_TabPage tabPage = tabList[i].tabPage;

            int idx = i;

            tabPage.Init();
            tabPage.SetSprite(defaultButtonSprite);
            tabPage.BindClick(() => Select(idx));
        }

        closeButton.onClick.AddListener(OnClose);
        resetButton.onClick.AddListener(OnResetSetting);
    }

    public void Open()
    {
        gameObject.SetActive(true);

        if (tabList.Count > 0)
            Select(0);
    }

    private void Select(int index)
    {
        if (index == currentIndex) 
            return;

        for (int i = 0; i < tabList.Count; i++)
        {
            TabItem tabItem = tabList[i];

            bool isActive = (i == index);

            if (tabItem.page != null)
                tabItem.page.SetActive(isActive);

            if (tabItem.tabPage != null)
                tabItem.tabPage.SetSprite(isActive ? activeButtonSprite : defaultButtonSprite);
        }

        currentIndex = index;
    }

    private void OnClose()
    {
        Managers.Setting.SaveSetting();
        gameObject.SetActive(false);
    }

    private void OnResetSetting()
    {
        Managers.Setting.ResetSettingData();
    }
}