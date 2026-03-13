using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatSelection : MonoBehaviour, ICharacterCreationSection
{
    [SerializeField] private List<UI_StatAllocation> statList;
    [SerializeField] private TextMeshProUGUI remainingStatText;
    [SerializeField] private UI_PlayerSubStatInfo playerSubStatInfo;

    private Dictionary<MainStatType, int> allocatedStatDic = new Dictionary<MainStatType, int>();          // 분배값

    private Dictionary<MainStatType, int> jobBaseMainStatDic = new Dictionary<MainStatType, int>();   // 직업 메인 스탯
    private Dictionary<SubStatType, float> jobBaseSubStatDic = new Dictionary<SubStatType, float>();  // 직업 서브 스탯

    private int startPoint;      // 시작 분배 포인트
    private string currentJobId; // 현재 선택된 직업 id 
    private int CurrentPoint { get => startPoint - UsedPoints; }  //남은 포인트

    private int UsedPoints
    {
        get
        {
            int used = 0;
            foreach (KeyValuePair<MainStatType, int> kv in allocatedStatDic) 
                used += kv.Value;

            return used;
        }
    }

    public void Init()
    {
        for (int i = 0; i < statList.Count; i++)
        {
            UI_StatAllocation statAllocation = statList[i];

            statAllocation.Init();

            statAllocation.OnClicked += ChangeAllocation;
        }

        playerSubStatInfo.Init();
    }

    public void ChangeAllocation(MainStatType type, int delta)
    {
        int allocated = allocatedStatDic[type];

        if (delta > 0)
        {
            if (CurrentPoint <= 0)
                return;

            allocated += 1;
        }
        else
        {
            if (allocated <= 0)
                return;

            allocated -= 1;
        }

        allocatedStatDic[type] = allocated;

        UpdateUi();

        return;
    }

    #region ICharacterCreationSection
    public void Refresh()
    {
        startPoint = 0;
        currentJobId = string.Empty;

        jobBaseMainStatDic.Clear();
        jobBaseSubStatDic.Clear();

        for (int i = 0; i < statList.Count; i++)
            allocatedStatDic[statList[i].MainStatType] = 0;

        UpdateUi();
    }

    public bool IsValid()
    {
        return CurrentPoint == 0;
    }

    public void Apply(GameStartData data)
    {
        data.str = GetFinalValue(MainStatType.Strength);
        data.agi = GetFinalValue(MainStatType.Dexterity);
        data.con = GetFinalValue(MainStatType.Constitution);
        data.intl = GetFinalValue(MainStatType.Intelligence);
        data.per = GetFinalValue(MainStatType.Perception);
    }
    #endregion

    #region 데이터 얻기
    public int GetFinalValue(MainStatType type)
    {
        return GetJobBonus(type) + allocatedStatDic[type];
    }

    private int GetJobBonus(MainStatType type)
    {
        if (jobBaseMainStatDic.TryGetValue(type, out int value))
            return value;

        return 0;
    }
    #endregion

    #region UI 갱신

    private void UpdateUi()
    {
        int currentPoint = CurrentPoint;

        remainingStatText.text = CurrentPoint.ToString();

        for (int i = 0; i < statList.Count; i++)
        {
            UI_StatAllocation stat = statList[i];
            MainStatType type = stat.MainStatType;

            int finalValue = GetFinalValue(type);
            int minValue = GetJobBonus(type);

            stat.SetValue(finalValue);
            stat.SetInteractableButtons(currentPoint, finalValue, minValue);
        }

        //선택된 직업이 없으면 보여주기X
        if (string.IsNullOrEmpty(currentJobId))
            playerSubStatInfo.Refresh(string.Empty);
        else
            playerSubStatInfo.Refresh(CalcuateTotalSubStat());
    }

    private IReadOnlyDictionary<SubStatType, float> CalcuateTotalSubStat()
    {
        Dictionary<SubStatType, float> resultDic = new Dictionary<SubStatType, float>();

        MainStatType[] mainStats = (MainStatType[])Enum.GetValues(typeof(MainStatType));
        for (int i = 0; i < mainStats.Length; i++)
        {
            MainStatType mainStatType = mainStats[i];

            int finalValue = GetFinalValue(mainStatType);

            MainStat mainStat = new MainStat(mainStatType, finalValue);

            //메인 스탯에 따른 서브 스탯 증가
            foreach (KeyValuePair<SubStatType, float> kv in mainStat.SubStatPerPointDic)
            {
                SubStatType sub = kv.Key;
                float perPoint = kv.Value;

                float add = finalValue * perPoint;

                if (resultDic.TryGetValue(sub, out float cur))
                    resultDic[sub] = cur + add;
                else
                    resultDic[sub] = add;
            }
        }

        //직업 기본 스탯 증가
        foreach (KeyValuePair<SubStatType, float> kv in jobBaseSubStatDic)
        {
            SubStatType sub = kv.Key;
            float value = kv.Value;

            resultDic[sub] += value;
        }

        return resultDic;
    }
    #endregion

    #region 외부 호출
    public void SetJob(PlayerSo playerSo)
    {
        startPoint = Managers.Game.StartPoint;
        currentJobId = playerSo.Id;

        foreach (KeyValuePair<MainStatType, int> kv in playerSo.MainStatDic)
            jobBaseMainStatDic[kv.Key] = kv.Value;

        foreach (KeyValuePair<SubStatType, float> kv in playerSo.SubStatDic)
            jobBaseSubStatDic[kv.Key] = kv.Value;
       
        UpdateUi();
    }
    #endregion
}