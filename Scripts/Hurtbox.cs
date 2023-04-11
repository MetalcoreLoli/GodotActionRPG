using Godot;
using System;

public partial class Hurtbox : Area2D
{
    #region Private memebers
    [Export] private EffectSpawner _hitEffectSpawner = null!;
    [Export] private bool _isHitEffectNeeded = true;
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

    private void _OnAreaEntered(Node2D area)
    {
        if (!_isHitEffectNeeded)
        {
            return;
        }

        _hitEffectSpawner.Spawn();
    }
    #endregion
}
