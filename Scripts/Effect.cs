using Godot;
using System;

public partial class Effect : AnimatedSprite2D
{
	#region Godot
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Frame = 0;
		Play("default");
        AnimationFinished += _OnAnimatedSprite2D_AnimationFinished;
	}

    public override void _ExitTree()
    {
        AnimationFinished -= _OnAnimatedSprite2D_AnimationFinished;
    }

    public void _OnAnimatedSprite2D_AnimationFinished()
    {
        QueueFree();
    }
    #endregion
}
