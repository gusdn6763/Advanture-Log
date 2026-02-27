using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class NeedModule
{
    private readonly BuffModule buff;
    private readonly IReadOnlyList<NeedData> needDatas;

    private readonly SerializedDictionary<NeedType, float> currentValues = new SerializedDictionary<NeedType, float>();
    private readonly SerializedDictionary<NeedType, NeedTier> currentTiers = new SerializedDictionary<NeedType, NeedTier>();

    public NeedModule(BuffModule buffModule)
    {
        buff = buffModule;
        //this.needDatas = Managers.Data.

        foreach (var needData in this.needDatas)
        {
            if (!currentValues.ContainsKey(needData.Type))
            {
                // 시작값을 사용하고, 범위 보정
                float start = Mathf.Clamp(needData.StartValue, needData.MinValue, needData.MaxValue);
                currentValues[needData.Type] = start;

                float percent = ToPercent(needData, start);
                currentTiers[needData.Type] = needData.GetTierFromValue(percent);
            }
        }

        Managers.Game.OnTimeAdvanced += Tick;
    }

    public void Tick(int minutes)
    {
        if (needDatas == null) return;

        foreach (var needData in needDatas)
        {
            float v = GetCurrentValue(needData.Type);
            v -= needData.ReducePerMinute * minutes;
            v = Mathf.Clamp(v, needData.MinValue, needData.MaxValue);
            currentValues[needData.Type] = v;

            float percent = ToPercent(needData, v);
            NeedTier newTier = needData.GetTierFromValue(percent);
            NeedTier oldTier = GetCurrentTier(needData.Type);

            if (newTier != oldTier)
            {
                currentTiers[needData.Type] = newTier;
                OnTierChanged(needData, oldTier, newTier);
            }
        }
    }

    private float ToPercent(NeedData nd, float value)
    {
        float range = nd.MaxValue - nd.MinValue;
        if (range <= 0f) return 100f;
        return Mathf.InverseLerp(nd.MinValue, nd.MaxValue, value) * 100f;
    }

    private float GetCurrentValue(NeedType type)
        => currentValues.TryGetValue(type, out var v) ? v : 0f;

    private NeedTier GetCurrentTier(NeedType type)
        => currentTiers.TryGetValue(type, out var t) ? t : NeedTier.Normal;

    private void OnTierChanged(NeedData nd, NeedTier oldTier, NeedTier newTier)
    {
        // TODO: 버프 연동 예시
        // var oldEffect = nd.GetEffectFromTier(oldTier);
        // var newEffect = nd.GetEffectFromTier(newTier);
        // if (oldEffect != null) buff.Remove(oldEffect, source: this);
        // if (newEffect != null) buff.Apply(newEffect, source: this);
    }

    // 외부 접근용 API (선택)
    public float GetValue(NeedType type) => GetCurrentValue(type);

    public void SetValue(NeedType type, float value)
    {
        var nd = FindNeed(type);
        if (nd == null) return;

        float v = Mathf.Clamp(value, nd.MinValue, nd.MaxValue);
        currentValues[type] = v;

        float percent = ToPercent(nd, v);
        currentTiers[type] = nd.GetTierFromValue(percent);
    }

    private NeedData FindNeed(NeedType type)
    {
        if (needDatas == null) return null;
        for (int i = 0; i < needDatas.Count; i++)
            if (needDatas[i].Type.Equals(type))
                return needDatas[i];
        return null;
    }
}