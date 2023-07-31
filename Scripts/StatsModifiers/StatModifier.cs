using System;

namespace ActionRPG.Scripts.StatsModifier;

public class StatModifier : IDisposable
{
    protected StatsComponent _statsComponents;
    protected StatModifier _next;

    public StatModifier(StatsComponent statsComponents)
    {
        _statsComponents = statsComponents ?? throw new NullReferenceException(nameof(statsComponents));
    }

    public void Add(StatModifier mod)
    {
        if (_next is not null)
        {
            _next.Add(mod);
            return;
        }
        _next = mod;
    }

    public virtual void Handle() => _next?.Handle();

    public virtual void Dispose()
    {
        return;
    }
}
