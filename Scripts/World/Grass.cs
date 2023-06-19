using Godot;
using System;

public partial class Grass : Node2D
{
	#region Private Members
	[Export] private EffectSpawner _grassEffectSpawner = null!;
	#endregion
	#region Godot
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _OnArea2D_AreaEntered(Area2D area)
	{
		GD.Print("Sword hit the grass");
		_grassEffectSpawner.Spawn();
		QueueFree();
	}
	#endregion
}
