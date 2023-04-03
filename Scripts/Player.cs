using Godot;
using System;

public partial class Player : CharacterBody2D
{
	#region Private members
	[Export] private float _speed = 80.0f;
	[Export] private float _acceleration = 500.0f;
	[Export] private float _friction = 500.0f;

	[Export(PropertyHint.Range, "-1,1")]
	private Vector2 _currentDirection = Vector2.Zero;

	[Export(PropertyHint.Flags, "Idle,Attack,Move,Roll")]
	private State _currentState = State.Idle;

	[Export] private AnimationTree _animationTree = null!;
	[Export] private AnimationPlayer _animationPlayer = null!;

	private AnimationNodeStateMachinePlayback _currentAnimationState = null!;

    #region State Implementations
	//TODO: move into a its own class
	private void MoveState(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		_currentDirection = direction;
		if (direction != Vector2.Zero)
		{
			_animationTree?.Set("parameters/Idle/blend_position", direction);
			_animationTree?.Set("parameters/Attack/blend_position", direction);
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

		// trigger for transition to the AttackState
		if (Input.IsActionJustPressed("Attack"))
		{
			_currentState = State.Attack;
		}
	}

	private void AttackState(double delta)
	{
		Velocity = Vector2.Zero;
		_currentAnimationState?.Travel("Attack");
		//_currentState = State.Idle;
	}

	private void RollState(double delta)
	{
		throw new NotImplementedException();
	}
    #endregion
	#endregion

	#region Properties
	public float Speed => _speed;
	public float Friction => _friction;
	public float Acceleration => _acceleration;
	#endregion

	#region Godot
	public override void _Ready()
	{
		_animationTree.Active = true;
		_currentAnimationState = (AnimationNodeStateMachinePlayback)(_animationTree?.Get("parameters/playback"));
		//_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	// public float Gravity => ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		switch (_currentState)
		{
			case State.Attack:
				AttackState(delta);
				break;
			case State.Idle: // handled inside of the MoveState method
			case State.Move:
				MoveState(delta);
				break;
			case State.Roll:
				RollState(delta);
				break;
			default:
				break;
		}
	}

    public void _OnArea2D_AreaEntered(Area2D area)
    {
    }
	// this method will be called at the end of attack animation
	public void AttackAnimationFinished()
	{
		_currentState = State.Idle;
	}
	#endregion

	[Flags]
	public enum State: byte
	{
		Idle = 1 << 1,
		Attack= 1 << 2,
		Move= 1 << 3,
		Roll= 1 << 4
	}
}
