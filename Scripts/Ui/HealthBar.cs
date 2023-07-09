using Godot;
using System;

public partial class HealthBar : TextureProgressBar
{
#region Private member
    [Export] private HealthComponent _healthComponent = null!;
#endregion
#region Godot stuff
    public override void _Ready()
    {
        MaxValue = _healthComponent.MaxHealth;
        Value = _healthComponent.Health;
        _healthComponent.OnHealthChanged += (value) => Value = value;
    }
#endregion
}
