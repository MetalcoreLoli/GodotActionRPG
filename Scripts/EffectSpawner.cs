using Godot;
using System;

public partial class EffectSpawner : Node2D
{
    #region Private members

    [Export] private PackedScene _effectTemplate = null!;

    public void Spawn()
    {
        if (_effectTemplate is null)
        {
            return;
        }
        var effect = _effectTemplate.Instantiate() as Node2D;
        var world = GetTree().CurrentScene;
        world.AddChild(effect);
        effect.GlobalPosition = GlobalPosition;
    }
    #endregion
}
