using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WeightOption
{
    public int Value;
    public int Weight;
}

[Serializable]
public sealed class WeightedTable
{
    [SerializeField] private List<WeightOption> options = new List<WeightOption>();

    public IReadOnlyList<WeightOption> Options => options;

    public WeightOption Pick()
    {
        int total = 0;

        for (int i = 0; i < options.Count; i++)
            total += Mathf.Max(0, options[i].Weight);

        int roll = UnityEngine.Random.Range(0, total);

        for (int i = 0; i < options.Count; i++)
        {
            int w = Mathf.Max(0, options[i].Weight);
            roll -= w;
            if (roll < 0)
                return options[i];
        }

        return options[options.Count - 1];
    }
}