using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class UI_Controls : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private SerializedDictionary<InputCategory, RectTransform> categoryTransformDic;

    [SerializeField] private UI_KeyButton keyButtonPrefab;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private LocalizedString warningString;

    private List<UI_KeyButton> keyButtons = new List<UI_KeyButton>();
    private UI_KeyButton waitingButton;
    private KeySetting keySetting;

    public void Init()
    {
        keySetting = Managers.Setting.KeySetting;
        Managers.Setting.OnReset += RefreshAllButtons;

        waitingButton = null;

        CreateButtons();
    }

    private void Update()
    {
        if (waitingButton == null)
            return;

        if (!Input.anyKeyDown)
            return;

        KeyCode pressedKey = GetPressedKey();
        if (pressedKey == KeyCode.None)
            return;

        ApplyKeyToWaitingButton(pressedKey);
    }

    private void OnEnable()
    {
        RefreshAllButtons();
    }

    private void OnDestroy()
    {
        Managers.Setting.OnReset -= RefreshAllButtons;
    }

    private void CreateButtons()
    {
        keyButtons.Clear();

        foreach (KeyValuePair<InputAction, InputActionData> kv in keySetting.KeyDic)
        {
            InputAction action = kv.Key;
            InputActionData data = kv.Value;

            if (!categoryTransformDic.TryGetValue(data.Category, out RectTransform transform))
            {
                Debug.LogError("InputCategory에 맞지 않는 키 버튼 존재");
                return;
            }    

            UI_KeyButton button = Instantiate(keyButtonPrefab, transform);
            button.Init(action, data);

            button.OnClicked += RequestRebind;

            keyButtons.Add(button);
        }
    }

    private void RefreshAllButtons()
    {
        for (int i = 0; i < keyButtons.Count; i++)
            keyButtons[i].Refresh();

        waitingButton = null;
    }

    private void ApplyKeyToWaitingButton(KeyCode newKey)
    {
        InputAction action = waitingButton.Action;
        KeyCode currentKey = keySetting.GetKeyCode(action);

        if (newKey == KeyCode.Escape)
        {
            ButtonCancle();
            return;
        }

        bool ok = keySetting.CheckKey(newKey, currentKey);
        if (!ok)
        {
            warningText.text = warningString.GetLocalizedString();
            return;
        }

        ButtonCancle();
        keySetting.AssignKey(newKey, action);
        waitingButton.ChangeKey(newKey);

        RefreshAllButtons();
    }

    private KeyCode GetPressedKey()
    {
        int min = (int)KeyCode.None;
        int max = (int)KeyCode.Menu;

        for (int i = min; i <= max; i++)
        {
            KeyCode code = (KeyCode)i;
            if (Input.GetKeyDown(code))
                return code;
        }

        return KeyCode.None;
    }

    #region 외부 호출
    public void RequestRebind(UI_KeyButton button)
    {
        //같은 버튼 클릭시 취소
        if (waitingButton != null || waitingButton == button)
            return;

        waitingButton = button;
    }

    private void ButtonCancle()
    {
        waitingButton.SetSelected(false);
        waitingButton = null;
        warningText.text = string.Empty;
    }
    #endregion
}