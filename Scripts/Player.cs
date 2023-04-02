using Godot;
using System;

public partial class Player : CharacterBody2D
{
	#region Private members
	[Export] private float _speed = 80.0f;
	[Export] private float _acceleration = 500.0f;
	[Export] private float _friction = 500.0f;

    [Export] private AnimationTree _animationTree = null!;
    [Export] private AnimationPlayer _animationPlayer = null!;

    private AnimationNodeStateMachinePlayback _currentAnimationState = null!;
	#endregion

	#region Properties
	public float Speed => _speed;
	public float Friction => _friction;
	public float Acceleration => _acceleration;
    #endregion

    #region Godot
    public override void _Ready()
    {
        _currentAnimationState = (AnimationNodeStateMachinePlayback)(_animationTree?.Get("parameters/playback"));
        //_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	// public float Gravity => ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
		{
            _animationTree?.Set("parameters/Idle/blend_position", direction);
            _animationTree?.Set("parameters/Run/blend_position", direction);
            _currentAnimationState?.Travel("Run");
			velocity = velocity.MoveToward(direction * Speed, Acceleration * (float)delta);
		}
		else
		{
            _currentAnimationState?.Travel("Idle");
			velocity = velocity.MoveToward(Vector2.Zero, Friction * (float)delta);
		}

		Velocity = velocity;
        _ = MoveAndSlide();
	}
	#endregion
}
