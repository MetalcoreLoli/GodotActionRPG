namespace ActionRPG.Scripts.StatsModifier;

public class KdModifier : StatModifier
{
    private readonly int _newValue;

    public KdModifier(StatsComponent statsComponents, int newValue) : base(statsComponents)
    {
        _newValue = newValue;
    }

    public override void Handle()
    {
        _statsComponents.Kd += _newValue;
        base.Handle();
    }

    public override void Dispose() => _statsComponents.Kd -= _newValue;
}
