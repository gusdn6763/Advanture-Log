using Data;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string settingPath;
    private string saveDir;

    public void Init()
    {
        saveDir = Path.Combine(Application.persistentDataPath, "Saves");
        settingPath = Path.Combine(Application.persistentDataPath, "SettingData.json");

        try
        {
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] SaveDir 생성 실패: {e}");
        }
    }

    #region 설정창
    public SettingData LoadSettingData(DefaultSettingSo defaultSettingData)
    {
        try
        {
            SettingData settingData = new SettingData();
            if (!File.Exists(settingPath))
                settingData.Init(defaultSettingData);
            else
            {
                string json = File.ReadAllText(settingPath, Encoding.UTF8);
                settingData = JsonConvert.DeserializeObject<SettingData>(json);
            }
            return settingData;
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] OptionData 로드 실패: {e}");
            return null;
        }
    }

    public bool SaveSettingData(SettingData data)
    {
        if (data == null)
            return false;

        try
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(settingPath, json);
            Debug.Log(settingPath + "의 위치에 저장했습니다.");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] OptionData 저장 실패: {e}");
            return false;
        }
    }
    #endregion

    #region 게임
    public bool SaveGame(int slot, GameSaveData data)
    {
        if (data == null)
            return false;

        string path = Path.Combine(saveDir, $"slot_{slot}.json");

        try
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log(path + "의 위치에 저장했습니다.");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] GameSave 저장 실패(slot {slot}): {e}");
            return false;
        }
    }
    #endregion
}