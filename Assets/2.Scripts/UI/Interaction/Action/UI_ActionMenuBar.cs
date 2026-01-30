using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ActionMenuBar : MonoBehaviour
{
    [SerializeField] private UI_ActionMenuButton actionMenuButtonPrefab;

    [SerializeField] private float defaultSize = 35f;
    [SerializeField] private float offset = 10f;
    [SerializeField] private int createCount = 5;

    private List<UI_ActionMenuButton> actionMenuButtons = new List<UI_ActionMenuButton>();
    private RectTransform rect;

    private float buttonHeight;
    private int currentIndex = 0;       //현재 타겟이 된 메뉴 순서
    private int activeCount = 0;        //BaseEntity의 메뉴 갯수

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        // 미리 일정 수 만큼만 만들어 둔다
        for (int i = 0; i < createCount; i++)
            actionMenuButtons.Add(CreateButton());

        gameObject.SetActive(false);
    }

    private UI_ActionMenuButton CreateButton()
    {
        UI_ActionMenuButton button = Instantiate(actionMenuButtonPrefab, transform);
        button.Init();

        return button;
    }

    public void OpenMenu(BaseEntity target)
    {
        List<ActionMenuSo> targetMenus = target.BaseData.ActionMenus;

        currentIndex = 0;
        activeCount = targetMenus.Count;
        gameObject.SetActive(true);

        //원래 메뉴보다 많으면 생성
        while (actionMenuButtons.Count < activeCount)
            actionMenuButtons.Add(CreateButton());

        // 앞에서부터 필요한 개수만 세팅 & 활성화
        buttonHeight = 0;
        for (int i = 0; i < actionMenuButtons.Count; i++)
        {
            UI_ActionMenuButton actionMenuButton = actionMenuButtons[i];
            ActionMenuSo targetMenu = targetMenus[i];

            buttonHeight += actionMenuButton.Height;

            actionMenuButton.gameObject.SetActive(true);
            actionMenuButton.SetText(targetMenu.ActionName.GetLocalizedString());
            actionMenuButton.Setup(targetMenu, target, i);
        }

        // 남는 버튼은 그냥 끈다
        for (int i = actionMenuButtons.Count; i < activeCount; i++)
            actionMenuButtons[i].gameObject.SetActive(false);

        // 메뉴 크기
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, defaultSize + activeCount * buttonHeight);


        SetMenuPosition(target.Position);
    }

    /// <summary>
    /// 메뉴 위치 설정
    /// </summary>
    /// <param name="worldPos"></param>
    private void SetMenuPosition(Vector2 worldPos)
    {
        float margin = 5f; // 화면 끝에서 좀 띄워줄 여백
        float menuW = rect.sizeDelta.x;
        float menuH = rect.sizeDelta.y;

        float halfW = menuW * 0.5f;
        float halfH = menuH * 0.5f;

        // 1. 타겟의 스크린 좌표
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // 2. "항상 오른쪽 아래" 기본 위치 (메뉴의 중심 좌표)
        Vector2 popupPos;
        popupPos.x = screenPos.x + halfW + offset;
        popupPos.y = screenPos.y - halfH - offset;

        // 3. 화면 안으로만 밀어넣기(clamp)
        //    - 방향은 유지: 즉, x는 여전히 screenPos.x보다 오른쪽에, y는 여전히 screenPos.y보다 아래에 있어야 합니다.
        //
        //    오른쪽으로 나가는 경우:
        //    popupPos.x + halfW  <= Screen.width - margin 이어야 함
        float maxX = Screen.width - margin - halfW;
        if (popupPos.x > maxX)
            popupPos.x = maxX;

        //    하지만 clamp 때문에 혹시나 왼쪽으로까지 넘어가면?
        //    (이론상 타겟이 화면 거의 오른쪽 끝에 붙은 초과상황에서만 가능)
        //    그래도 "오른쪽 아래" 컨셉은 유지해야 하므로
        //    최소값은 여전히 타겟 오른쪽보다 약간은 오른쪽에 있어야 의미가 있겠죠.
        //    즉, popupPos.x는 최소한 screenPos.x + halfW*0.2f 정도는 유지시키는 식으로 살짝 제약을 줄 수 있습니다.
        //    (0.2f 같은 값은 감각 보정용. 완전 겹치지 않게.)
        float minX = screenPos.x + halfW * 0.2f;
        if (popupPos.x < minX)
            popupPos.x = minX;

        //    아래로 나가는 경우:
        //    popupPos.y - halfH >= margin 이어야 함
        float minY = margin + halfH;
        if (popupPos.y < minY)
            popupPos.y = minY;

        //    위쪽으로 너무 올라갔을 때(=오브젝트가 화면 아~주 아래쪽인 상황에서 clamp가 너무 밀어올릴 수도 있음)
        //    그래도 "아래쪽" 느낌을 유지하고 싶다면,
        //    popupPos.y는 screenPos.y - halfH*0.2f 이하까지만 허용.
        //    즉 오브젝트보다 위로는 거의 안 올라가게 살짝 눌러줍니다.
        float maxY = screenPos.y - halfH * 0.2f;
        if (popupPos.y > maxY)
            popupPos.y = maxY;

        // 4. 실제 적용
        rect.position = popupPos;

        SetIndex(0);
    }

    public void SetIndex(int newIndex)
    {
        if (newIndex < 0)
            newIndex = activeCount - 1;
        if (newIndex >= activeCount) 
            newIndex = 0;

        currentIndex = newIndex;

        // 이전 하이라이트 해제
        for (int i = 0; i < activeCount; i++)
            actionMenuButtons[i].SetHighlighted(false);

        // 신규 하이라이트
        actionMenuButtons[currentIndex].SetHighlighted(true);
    }

    public void NavigateMenu(bool isUp)
    {
        if (isUp)
            SetIndex(currentIndex - 1);
        else
            SetIndex(currentIndex + 1);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
