using Godot;
using System;

public partial class Grass : Node2D
{
    #region Private Members
    [Export] private PackedScene _destroyEffectTemplate = null!;
    #endregion
    #region Godot
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsActionJustPressed("Attack"))
        {
            var effect = _destroyEffectTemplate.Instantiate() as Node2D;
            var world = GetTree().CurrentScene;
            world.AddChild(effect);
            effect.GlobalPosition = GlobalPosition;
            QueueFree();
        }
	}
    #endregion
}
