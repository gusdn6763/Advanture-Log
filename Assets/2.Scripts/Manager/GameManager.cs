using Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void Init()
    {
        OnDayChanged += HandleDayChanged_AutoSave;
    }

    #region 시간
    public event Action<int> OnTimeAdvanced;          // 지나간 시간
    public event Action<int> OnDayChanged;            // 날짜 변경시
    public event Action<int, int> OnClockChanged;     // 현재 시간

    public int LastAutoSaveDay { get; set; }
    public int Day { get; private set; }
    public int Hour { get; private set; }
    public int Minute { get; private set; }

    public void AdvanceMinutes(int minutes)
    {
        if (minutes <= 0)
            return;

        int totalMinutes = (Day - 1) * 1440 + Hour * 60 + Minute + minutes;

        // 전체 경과 일수와 하루 내 분 계산
        int newDayIndex = totalMinutes / 1440;
        int minutesOfDay = totalMinutes % 1440;

        int oldDay = Day;
        Day = newDayIndex + 1;

        // 하루가 넘어간 경우에만 이벤트 발생
        if (Day != oldDay)
            OnDayChanged?.Invoke(Day);

        Hour = minutesOfDay / 60;
        Minute = minutesOfDay % 60;

        OnTimeAdvanced?.Invoke(minutes);
        OnClockChanged?.Invoke(Hour, Minute);
    }
    public int GetTotalTime()
    {
        return (Day - 1) * 1440 + (Hour * 60) + (Minute);
    }
    public void HandleDayChanged_AutoSave(int newDay)
    {
        int interval = Managers.Setting.GameplaySetting.AutoSavePeriod;

        if (interval <= 0)
            return;

        // lastAutoSaveDay 초기화 정책:
        // - 로드게임이면 ApplySaveData에서 복원하는 게 최선
        // - 새 게임이면 1일 시작 기준으로 잡는 게 보통
        if (LastAutoSaveDay <= 0)
            LastAutoSaveDay = newDay;

        int passed = newDay - LastAutoSaveDay;
        if (passed < interval)
            return;

        // 실제 저장은 SaveManager(IO) 호출
        bool ok = SaveGame(); // 아래 SaveGame 구현 참고
        if (ok)
            LastAutoSaveDay = newDay;
    }
    /// <summary>
    /// 총 분 -> (일, 시, 분)
    /// </summary>
    /// <param name="totalMinutes"></param>
    /// <param name="days"></param>
    /// <param name="hours"></param>
    /// <param name="minutes"></param>
    public static void SplitMinutesDHM(int totalMinutes, out int days, out int hours, out int minutes)
    {
        totalMinutes = Mathf.Max(0, totalMinutes);
        days = totalMinutes / 1440;
        int rem = totalMinutes % 1440;
        hours = rem / 60;
        minutes = rem % 60;
    }
    public void AutoSavePeriodChanged()
    {
        LastAutoSaveDay = Day;
    }

    #endregion

    #region 기타
    public int StartPoint { get; set; } = 5;        //초기 설정 가능한 스탯 포인트
    public int DefaultAccuracy { get; set; } = 50;  //기본 맞을 확률
    #endregion

    #region 저장
    public int CurrentGameSlot { get; private set; }

    public bool SaveGame()
    {
        GameSaveData data = new GameSaveData();
        //data.TimeData = 0;

        return Managers.Save.SaveGame(CurrentGameSlot, data);
    }

    #endregion
}

