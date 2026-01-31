using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ActionMenuOverlay : MonoBehaviour, IPointerDownHandler
{
    private UI_ActionMenuBar actionMenuBar;

    public void Init()
    {
        actionMenuBar = GetComponentInChildren<UI_ActionMenuBar>();

        actionMenuBar.Init();
        gameObject.SetActive(false);
    }

    public void Open(BaseEntity target)
    {
        gameObject.SetActive(true);              // 블로커(부모) ON
        actionMenuBar.OpenMenu(target);          // 내용 세팅
    }

    public void Close()
    {
        gameObject.SetActive(false);         // 블로커(부모) OFF => 자식 자동 OFF
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Close();
    }
}