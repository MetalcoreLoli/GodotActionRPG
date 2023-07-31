using Godot;

public partial class EffectSpawner : Node2D
{
    #region Private members

    [Export] private PackedScene _effectTemplate = null!;

    public void SpawnAt(Vector2 position)
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

    public void Spawn() => SpawnAt(GlobalPosition);
    #endregion
}
