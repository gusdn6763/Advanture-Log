//using UnityEngine;

//[CreateAssetMenu(menuName = "Buffs/Well Rested")]
//public class WellRestedEffectSO : EffectSO
//{
//    [Header("버프 최소 시간")]
//    [Range(1, 12)] public int buffMinHour = 6;

//    [Header("마지막으로 버프를 받은 시간")]
//    private int lastEffectTime = 0;

//    [Header("지속시간(분)")]
//    public float defaultDurationMinutes = 0f;

//    [Header("정수 자원 보정(적용 시 1회)")]
//    public int hungerDeltaOnApply = 0; // 음수면 허기 증가(소모), 양수면 회복
//    public int sleepDeltaOnApply = 0; // 수면/피로 정수 보정

//    private float? _overrideDuration;

//    public void TryApplyWellRestedToPlayer(int hours)
//    {
//        if (hours < buffMinHour)
//            return;

//        //버프를 받고 24시간이 지나면 가능
//        int totalTime = Managers.Game.GetTotalTime();
//        if (totalTime > lastEffectTime + 1440)
//            return;

//        lastEffectTime = totalTime;
//    }

//    public void Apply(IEffectable actor)
//    {
//        if (actor == null)
//            return;

//        float duration = Mathf.Max(0f, _overrideDuration ?? defaultDurationMinutes);
//    }

//    public void Remove(IEffectable actor)
//    {
//        if (actor == null)
//            return;
//    }
//}