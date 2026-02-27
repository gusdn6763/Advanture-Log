using System.Collections.Generic;
using UnityEngine;

public class UI_ActionMenuBar : UI_PopupBase
{
    [SerializeField] private UI_ActionMenuButton actionMenuButtonPrefab;

    [SerializeField] private int createCount = 5;

    private List<UI_ActionMenuButton> actionMenuButtons = new List<UI_ActionMenuButton>();
    private KeySetting keySetting;
    private RectTransform rect;

    public override bool Init()
    {
        if (!base.Init())
            return false;

        // 미리 캐싱
        keySetting = Managers.Setting.KeySetting;

        rect = GetComponent<RectTransform>();

        // 미리 일정 수 만큼만 만들어 둔다
        for (int i = 0; i < createCount; i++)
            actionMenuButtons.Add(CreateButton());

        return true;
    }

    private UI_ActionMenuButton CreateButton()
    {
        UI_ActionMenuButton button = Instantiate(actionMenuButtonPrefab, transform);
        button.Init();

        return button;
    }

    public void OpenMenu(BaseEntity target, IReadOnlyList<ActionMenuSo> targetMenus)
    {
        //원래 메뉴보다 많으면 생성
        while (actionMenuButtons.Count < targetMenus.Count)
            actionMenuButtons.Add(CreateButton());

        // 앞에서부터 필요한 개수만 세팅 & 활성화
        for (int i = 0; i < targetMenus.Count; i++)
        {
            UI_ActionMenuButton actionMenuButton = actionMenuButtons[i];
            ActionMenuSo targetMenu = targetMenus[i];

            actionMenuButton.gameObject.SetActive(true);

            InputActionData inputActionData = keySetting.GetInputActionData(targetMenu.InputAction);

            if (inputActionData != null)
                actionMenuButton.SetText($"{inputActionData.DisplayName.GetLocalizedString()} {inputActionData.keyCode}");
            else
                actionMenuButton.SetText(string.Empty);

            actionMenuButton.Setup(targetMenu, target);
        }

        // 남는 버튼은 그냥 끈다
        for (int i = actionMenuButtons.Count - 1; i >= targetMenus.Count; i--)
            actionMenuButtons[i].gameObject.SetActive(false);

        SetMenuPosition(target.transform.position);
    }

    /// <summary>
    /// 메뉴 위치 설정
    /// </summary>
    /// <param name="worldPos"></param>
    private void SetMenuPosition(Vector2 worldPos)
    {
        float margin = 5f;

        // 1. 타겟의 스크린 좌표
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        //현재 위치 + 메뉴 크기가 스크린 가로 범위를 넘어간다면
        if ((screenPos.x + rect.sizeDelta.x) - (Screen.width - margin) > 0f)
            screenPos.x -= rect.sizeDelta.x;

        //현재 위치 + 메뉴 세로 크기가 스크린 세로 범위를 넘어간다면
        if (margin - (screenPos.y - rect.sizeDelta.y) > 0f)
            screenPos.y += rect.sizeDelta.y;

        // 4. 실제 적용
        rect.position = screenPos;
    }
}
