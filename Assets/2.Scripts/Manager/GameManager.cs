using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region GameSaveData
    GameSaveData saveData = new GameSaveData();
    public GameSaveData SaveData { get { return saveData; } set { saveData = value; } }
    #endregion

    #region 시간
    public event Action<int> OnTimeAdvanced;           // 지나간 시간
    public event Action<int> OnDayChanged;            // 날짜 변경시
    public event Action<int, int> OnClockChanged;     // 현재 시간

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
    #endregion
}
