using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_StatSelection : MonoBehaviour, ICharacterCreationSection
{
    [SerializeField] private List<UI_StatAllocation> statList;
    [SerializeField] private TextMeshProUGUI remainingStatText;
    [SerializeField] private TextMeshProUGUI mainStatExplanText;
    [SerializeField] private TextMeshProUGUI subStatExplanText;

    private Dictionary<MainStatType, int> allocatedStatDic = new Dictionary<MainStatType, int>();           // 분배값 => 직업을 바꿔도 분배한 정보는 유지
    private IReadOnlyDictionary<MainStatType, int> jobBonusStatDic = new Dictionary<MainStatType, int>();   // 직업 보정

    private int startPoint;     // 시작 분배 포인트
    private int startStat;      // 기본 스탯
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
        PlayerRuleSo rule = Managers.Data.PlayerRule;

        startPoint = rule.StartPoint;
        startStat = rule.StartStat;

        for (int i = 0; i < statList.Count; i++)
        {
            UI_StatAllocation statAllocation = statList[i];
            statAllocation.Init();

            statAllocation.OnHovered += UpdateMainStatExplan;
            statAllocation.OnClicked += ChangeAllocation;
        }
    }

    private void UpdateMainStatExplan(string str)
    {
        mainStatExplanText.text = str;
    }

    public void ChangeAllocation(MainStatType type, int delta)
    {
        int allocated = GetAllocated(type);

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

    private void UpdateUi()
    {
        int currentPoint = CurrentPoint;

        remainingStatText.text = CurrentPoint.ToString();

        for (int i = 0; i < statList.Count; i++)
        {
            var stat = statList[i];
            var type = stat.MainStatType;

            int finalValue = GetFinalValue(type);
            int minValue = GetMinValue(type);

            stat.SetValue(finalValue);
            stat.SetInteractableButtons(currentPoint, finalValue, minValue);
        }

        UpdateSubStatExplan();
    }

    #region ICharacterCreationSection
    public void Refresh()
    {
        allocatedStatDic.Clear();
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
        return startStat + GetJobBonus(type) + GetAllocated(type);
    }

    /// <summary>
    /// 초기 스탯 및 직업으로 더해진 스탯으로 인한 값
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public int GetMinValue(MainStatType type)
    {
        return startStat + GetJobBonus(type);
    }

    public int GetAllocated(MainStatType type)
    {
        if (allocatedStatDic.TryGetValue(type, out int value))
            return value;

        return 0;
    }

    private int GetJobBonus(MainStatType type)
    {
        if (jobBonusStatDic.TryGetValue(type, out int value))
            return value;

        return 0;
    }
    #endregion

    #region 서브 스탯 보여주기
    private void UpdateSubStatExplan()
    {
        StatRuleSo statRule = Managers.Data.StatRule;
        
        IReadOnlyDictionary<SubStatType, float> totalSubStatDic = CalcuateTotalSubStat(statRule);

        subStatExplanText.text = DrawAllSubStatState(statRule.SubStatDic, totalSubStatDic);
    }

    private IReadOnlyDictionary<SubStatType, float> CalcuateTotalSubStat(StatRuleSo statRule)
    {
        Dictionary<SubStatType, float> resultDic = new Dictionary<SubStatType, float>();

        MainStatType[] mainStats = (MainStatType[])Enum.GetValues(typeof(MainStatType));
        for (int i = 0; i < mainStats.Length; i++)
        {
            MainStatType mainStat = mainStats[i];

            if (!statRule.TryGet(mainStat, out MainStatRule MainStatRule))
                continue;

            int finalValue = GetFinalValue(mainStat);

            foreach (KeyValuePair<SubStatType, float> kv in MainStatRule.SubStatPerPointDic)
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
        return resultDic;
    }
    private string DrawAllSubStatState(IReadOnlyDictionary<SubStatType, SubStatRule> subStatRule, IReadOnlyDictionary<SubStatType, float> totalSubStatDic)
    {
        StringBuilder sb = new StringBuilder(256);

        int idx = 0;
        foreach (var kv in subStatRule)
        {
            SubStatType type = kv.Key;
            SubStatRule rule = kv.Value;

            totalSubStatDic.TryGetValue(type, out float value);

            string name = rule.StatName != null ? rule.StatName.GetLocalizedString() : type.ToString();

            sb.Append(name);
            sb.Append(": ");
            sb.Append(StringUtil.FormatValueForDisplay(value, rule.DisplayType));

            if (idx < subStatRule.Count - 1)
                sb.Append('\n');

            idx++;
        }

        return sb.ToString();
    }
#endregion

#region 외부 호출
public void SetJobId(string id)
    {
        if (!Managers.Data.TryGetEntity(id, out PlayerSo so))
        {
            Debug.LogError($"플레이어 Id오류: {id}");
            return;
        }

        jobBonusStatDic = so.BaseMainStat;

        UpdateUi();
    }
    #endregion
}