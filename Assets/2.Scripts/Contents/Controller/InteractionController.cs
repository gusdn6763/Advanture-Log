using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private GameObject selectBar;

    private BaseEntity currentSelectEntity;

    private void Update()
    {
        UpdateSelectBar();

        HandleInput();
    }

    private void OnEnable()
    {
        Managers.Area.OnAreaChanged += SelectBarReset;
    }

    private void OnDisable()
    {
        Managers.Area.OnAreaChanged -= SelectBarReset;
    }

    private void UpdateSelectBar()
    {
        if (currentSelectEntity == null)
        {
            if (selectBar.activeSelf)
                selectBar.SetActive(false);

            return;
        }
        else if (!currentSelectEntity.gameObject.activeInHierarchy) //비활성화
        {
            currentSelectEntity = null;

            if (selectBar.activeSelf)
                selectBar.SetActive(false);

            return;
        }

        if (!selectBar.activeSelf)
            selectBar.SetActive(true);

        selectBar.transform.position = currentSelectEntity.Position;
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
            currentSelectEntity = FindEntityUnderMouse();
        else if (Input.GetMouseButtonDown(1))
        {
            BaseEntity hit = FindEntityUnderMouse();
            if (hit != null && hit == currentSelectEntity)
                Managers.UI.OpenUi(UiType.ActionMenu, hit);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Managers.UI.ClosePopupUI();
    }

    private BaseEntity FindEntityUnderMouse()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && hit.collider.TryGetComponent(out BaseEntity target))
            return target;

        return null;
    }

    public void SelectBarReset(Area area)
    {
        currentSelectEntity = null;
        selectBar.SetActive(false);
    }
}