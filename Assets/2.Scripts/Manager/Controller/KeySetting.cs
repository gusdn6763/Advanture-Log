using System.Collections.Generic;
using UnityEngine;

//https://gist.github.com/Extremelyd1/4bcd495e21453ed9e1dffa27f6ba5f69

public class KeySetting : MonoBehaviour
{
    private Dictionary<InputAction, InputActionData> keyDic = new Dictionary<InputAction, InputActionData>();

    public IReadOnlyDictionary<InputAction, InputActionData> KeyDic { get => keyDic; }

    public void Init(DefaultSettingSo defaultSetting)
    {
        keyDic.Clear();

        foreach (KeyValuePair<InputAction, InputActionData> kv in defaultSetting.InputActionDic)
            keyDic[kv.Key] = new InputActionData(kv.Value);
    }

    public KeyCode GetKeyCode(InputAction keyName)
    {
        if (keyDic.TryGetValue(keyName, out InputActionData inputActionData))
            return inputActionData.keyCode;

        return KeyCode.None;
    }

    public InputActionData GetInputActionData(InputAction keyName)
    {
        if (keyDic.TryGetValue(keyName, out InputActionData inputActionData))
            return inputActionData;

        return null;
    }

    /// <summary>
    /// 해당 키에서 자기 자신을 제외한 키가 등록되어있는경우를 방지하고, 특정한 키 설정을 방지하기위해 키를 체크한다.
    /// </summary>
    /// <returns>할당 가능한 키인가?</returns>
    public bool CheckKey(KeyCode key, KeyCode currentKey)
    {
        if (currentKey == key)
            return true;

        //1차 키 검사. 
        //키는 아래의 키만 허용한다.
        if
        (key >= KeyCode.A && key <= KeyCode.Z || //97 ~ 122   A~Z
            key >= KeyCode.Alpha0 && key <= KeyCode.Alpha9 || //48 ~ 57    알파 0~9
            key == KeyCode.Quote || //39         
            key == KeyCode.Comma || //44
            key == KeyCode.Period || //46
            key == KeyCode.Slash || //47
            key == KeyCode.Semicolon || //59
            key == KeyCode.LeftBracket || //91
            key == KeyCode.RightBracket || //93
            key == KeyCode.Minus || //45
            key == KeyCode.Equals || //61
            key == KeyCode.BackQuote //96
        ) { }
        else
            return false;

        //현재 설정된 키들 중 이미 할당된 키가 있는경우는 설정할 수 없다.
        foreach (KeyValuePair<InputAction, InputActionData> kv in keyDic)
            if (kv.Value.keyCode == key)
                return false;

        //모든 키 검사를 통과하면 해당 키는 설정이 가능한 키.
        return true;
    }

    public void AssignKey(KeyCode keyCode, InputAction action)
    {
        if (keyDic.TryGetValue(action, out InputActionData inputActionData))
            inputActionData.keyCode = keyCode;
        else
            Debug.LogError("존재하지 않는 InputAction");
    }

    #region 저장 및 로드
    public void Load(SettingData settingData)
    {
        foreach (KeyData keyData in settingData.keyData)
        {
            if (keyDic.TryGetValue(keyData.inputAction, out InputActionData inputActionData))
                inputActionData.keyCode = keyData.keyCode;
            else
                Debug.LogError("생성되지 않았거나 존재하지 않는 InputAction");
        }
    }

    public void SaveTo(SettingData data)
    {
        data.keyData.Clear();
        data.keyData = new List<KeyData>();

        foreach (KeyValuePair<InputAction, InputActionData> kv in keyDic)
            data.keyData.Add(new KeyData(kv.Key, kv.Value.keyCode));
    }
    #endregion
}