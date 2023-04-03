using Godot;
using System;

public partial class GrassEffect : Node2D
{
    #region Private Members
    [Export] private AnimatedSprite2D _effect = null!;
    #endregion
    #region Godot
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _effect.Frame = 0;
        _effect.Play("Animate");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void OnAnimatedSprite_AnimationFinished()
    {
        QueueFree();
    }
    #endregion
}
