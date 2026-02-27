using System;
using System.Collections.Generic;

public class EffectInstance
{
    //public EffectSO Effect { get; }
    //public object Source { get; }
    //public int RemineMinuate { get; }

    //public EffectInstance(EffectSO effect, object source)
    //{
    //    Effect = effect;
    //    Source = source;
    //}
}


public class BuffModule
{
    private readonly ResourceModule resource;

    private readonly List<EffectInstance> activeEffect = new List<EffectInstance>();

    public BuffModule(ResourceModule resource)
    {
        this.resource = resource;
    }

    public void ApplyEffect()//EffectSO effectSO)
    {
        //EffectInstance effectInstance = new EffectInstance(effectSO, )
    }

    public void RemoveBySource(object source)
    {
        for (int i = activeEffect.Count - 1; i >= 0; i--)
        {
            //if (Equals(activeEffect[i].Source, source))
                //activeEffect.RemoveAt(i);
        }
    }
}