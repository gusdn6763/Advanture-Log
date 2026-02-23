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
    public bool TryLoadSettingData(out SettingData data)
    {
        data = null;

        try
        {
            if (!File.Exists(settingPath))
                return false;

            string json = File.ReadAllText(settingPath, Encoding.UTF8);
            data = JsonConvert.DeserializeObject<SettingData>(json);

            // 역직렬화 실패(파일 손상 등)
            return data != null;
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] OptionData 로드 실패: {e}");
            data = null;
            return false;
        }
    }

    public bool SaveSettingData(SettingData data)
    {
        if (data == null)
            return false;

        try
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            return WriteAtomic(settingPath, json);
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
            return WriteAtomic(path, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] GameSave 저장 실패(slot {slot}): {e}");
            return false;
        }
    }
    #endregion

    #region 공통 유틸 - 안전 저장
    private bool WriteAtomic(string path, string contents)
    {
        // tmp에 쓰고 → 교체
        string tmpPath = path + ".tmp";

        try
        {
            File.WriteAllText(tmpPath, contents, Encoding.UTF8);

            if (File.Exists(path))
                File.Delete(path);

            File.Move(tmpPath, path);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] WriteAtomic 실패: {e}");

            // tmp가 남았으면 정리 시도
            try
            {
                if (File.Exists(tmpPath)) 
                    File.Delete(tmpPath);
            }
            catch
            {
            }

            return false;
        }
    }
    #endregion
}