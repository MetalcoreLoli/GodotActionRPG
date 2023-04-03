using Godot;
using System;

public partial class EnemyDeathEffect : Node2D
{
    #region Private members

    [Export] private AnimatedSprite2D _effectAnimatedSprite = null!;
    #endregion

    #region Godot
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        if (_effectAnimatedSprite is null) return;

        _effectAnimatedSprite.Frame = 0;
        _effectAnimatedSprite.Play("Animate");
	}

    public void _OnAnimatedSprite2D_AnimationFinished()
    {
        QueueFree();
    }
    #endregion
}
