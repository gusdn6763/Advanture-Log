using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

//https://gist.github.com/Extremelyd1/4bcd495e21453ed9e1dffa27f6ba5f69

[Serializable]
public class KeyData
{
    public InputAction inputAction;

    public KeyCode keyCode;

    public KeyData(InputAction inputAction, KeyCode keyCode)
    {
        this.inputAction = inputAction;
        this.keyCode = keyCode;
    }
}

public class KeyManager : MonoBehaviour
{
    [SerializeField] private InputBindingSo inputBindingSo; //초기 데이터

    private string mOptionDataFileName = "/KeyData.json";
    private string mFilePath;

    private Dictionary<InputAction, KeyCode> keyDic = new Dictionary<InputAction, KeyCode>();

    void Awake()
    {
        mFilePath = Application.persistentDataPath + mOptionDataFileName;

        LoadOptionData();
    }

    private void LoadOptionData()
    {
        if (File.Exists(mFilePath))
        {
            string fromJsonData = File.ReadAllText(mFilePath);

            List<KeyData> keyList = JsonConvert.DeserializeObject<List<KeyData>>(fromJsonData);

            foreach (KeyData data in keyList)
                keyDic.Add(data.inputAction, data.keyCode);
        }
        else
        {
            Debug.Log(GetType() + " 파일 없음");

            ResetOptionData();
        }
    }

    private void ResetOptionData()
    {
        keyDic.Clear();

        foreach (KeyValuePair<InputAction, KeyCode> key in inputBindingSo.KeyDic)
            keyDic.Add(key.Key, key.Value);

        Debug.Log(GetType() + " 초기화");

        SaveOptionData();
    }

    public void SaveOptionData()
    {
        List<KeyData> keys = new List<KeyData>();

        foreach (KeyValuePair<InputAction, KeyCode> keyName in keyDic)
            keys.Add(new KeyData(keyName.Key, keyName.Value));

        string jsonData = JsonConvert.SerializeObject(keys);

        //파일로 쓰기
        FileStream fileStream = new FileStream(mFilePath, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();

        Debug.Log(GetType() + " 파일 쓰기");
    }

    public KeyCode GetKeyCode(InputAction keyName)
    {
        return keyDic[keyName];
    }

    /// <summary>
    /// 해당 키에서 자기 자신을 제외한 키가 등록되어있는경우를 방지하고, 특정한 키 설정을 방지하기위해 키를 체크한다.
    /// </summary>
    /// <returns>할당 가능한 키인가?</returns>
    public bool CheckKey(KeyCode key, KeyCode currentKey)
    {
        //예외1. 현재 할당된 키에 같은 키로 설정하도록 한 경우는 허용으로 리턴한다.
        if (currentKey == key) 
            return true;

        //1차 키 검사. 
        //키는 아래의 키만 허용한다.
        if
        (
            key >= KeyCode.A && key <= KeyCode.Z || //97 ~ 122   A~Z
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
        foreach (KeyValuePair<InputAction, KeyCode> keyPair in keyDic)
            if (key == keyPair.Value)
                return false;

        //모든 키 검사를 통과하면 해당 키는 설정이 가능한 키.
        return true;
    }

    public void AssignKey(KeyCode keyCode, InputAction keyName)
    {
        //딕셔너리 
        keyDic[keyName] = keyCode;

        //키 파일을 로컬에 저장
        SaveOptionData();
    }
}